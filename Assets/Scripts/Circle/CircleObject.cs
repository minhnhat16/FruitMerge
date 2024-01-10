using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CircleObject : FSMSystem
{
    [HideInInspector] public SpawnState Spawn;
    [HideInInspector] public DropState Drop;
    [HideInInspector] public MergeState Merge;
    [HideInInspector] public GroundedState Grounded;
    private CircleTypeConfigRecord record;
    private Rigidbody2D ridBody;
    [SerializeField]
    private CircleCollider2D col;
    private CircleObject contactCircle;
    private int typeID;
    private float instanceID;
    private float fallSpeed;
    private float smoothTime;
    [SerializeField] private bool isDropping;
    [SerializeField] private bool isMerged;
    public Vector2 force;

    [SerializeField]
    private GameObject mergeVfx;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public int TypeID { get { return typeID; } }
    public bool IsMerged { get { return IsMerged; } }
    public bool IsDropping { get { return isDropping; } }

    public Rigidbody2D RigBody { get { return ridBody; } }
    public CircleObject ContactCircle { get { return contactCircle; } }
    public void SetIsDropping(bool isDropping)
    {
        this.isDropping = isDropping;
    }
    public void SetColliderRadius(float radius)
    {
        col.radius = radius;
    }
    public void SetIsMerge(bool isMerge)
    {
        this.isMerged = isMerge;
    }
    public void SetTypeID(int typeID)
    {
        this.typeID = typeID;
    }
    private void Awake()
    {
        Spawn.Setup(this);
        Drop.Setup(this);
        Merge.Setup(this);
        Grounded.Setup(this);
        ridBody = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    // Start is called before the first frame update
    public void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        ClaimPosition();
        StartCoroutine(ResetMerge());
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //ContactPoint2D[] contacts = collision.contacts;
        //for each (ContactPoint2D contact in contacts)
        //{
        if (/*contact.collider.*/collision.gameObject.CompareTag("MergeCircle") && isDropping == false)
        {
            instanceID = Time.time;
            CircleObject otherCircle = collision.gameObject.GetComponent<CircleObject>();

            if (isMerged || otherCircle.isMerged)
                return;
            contactCircle = collision.gameObject.GetComponent<CircleObject>();
            SwitchCircleOption(otherCircle);
            return;
        }
        else if (collision.gameObject.CompareTag("Topwall") && transform.position.y > 8)
        {
            IngameController.instance.isGameOver = true;
        }

    }
    IEnumerator ResetMerge()
    {
        yield return new WaitForSeconds(2f);
        isMerged = false;
    }
    public void ClaimPosition()
    {
        float x = transform.position.x;
        x = Mathf.Clamp(x, CameraMain.instance.GetLeft(), CameraMain.instance.GetRight());
        transform.position = new Vector3(x, transform.position.y);
    }
    public virtual void SwitchCircleOption(CircleObject otherCircle)
    {


        if (typeID == 11) return; //return when reach maximum
        if (typeID != contactCircle.GetComponent<CircleObject>().typeID)
        {
            return;
        }
        else
        {

            if (instanceID > contactCircle.GetComponent<CircleObject>().instanceID) return;
            else
            {
                GotoState(Merge);
                isMerged = true;
                otherCircle.isMerged = true;
            }
        }

    }
    public void PopAroundCircle()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius);
        foreach (Collider2D col in hits)
        {
            if (col.gameObject != this.gameObject)
            {
                Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = (col.transform.position - transform.position);
                    rb.AddForce(direction * 1f, ForceMode2D.Impulse);
                }
            }
        }

    }
    [Serialize] Tween tween;
    public IEnumerator SpawnNewCircle(GameObject col)
    {
        int t = typeID + 1;
        tween = col.transform.DOMove(transform.position, 0.2f);
        tween.OnComplete(() => tween?.Kill());
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col.GetComponent<Collider2D>());

        yield return new WaitForSeconds(0.25f);
        EndlessLevel.Instance.RemoveCircle(col.GetComponent<CircleObject>());
        EndlessLevel.Instance.RemoveCircle(this);
        
        col.SetActive(false);
        gameObject.SetActive(false);
        var c = CirclePool.instance.pool.SpawnNonGravity();
        c.SetTypeID(t);
        c.transform.localScale = Vector3.zero;
        c.SpawnCircle(t);
        c.record = ConfigFileManager.Instance.CircleConfig.GetRecordByKeySearch(c.typeID - 1);
        c.col.GetComponent<CircleCollider2D>().radius = c.record.Radius;
        c.mergeVfx.GetComponent<ParticleSystem>().Play();
        c.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        c.ridBody.bodyType = RigidbodyType2D.Dynamic;
        PopAroundCircle();
        int score = typeID + c.typeID;
        IngameController.instance.AddScore(score);
        EndlessLevel.Instance.AddCircle(c);
        EndlessLevel.Instance.FindLargestType(typeID + 1);
    }
    public void SpawnCircle(int i)
    {
        //Debug.Log("Spawn Circle" + type.spriteType[i - 1] + " " + type.scale[i - 1]);
        //int i = intQueue.Dequeue();
        instanceID = 0;
        //spriteRenderer.sprite = type.spriteType[i - 1];
        record = ConfigFileManager.Instance.CircleConfig.GetRecordByKeySearch(i);
        //Debug.Log("RECORD ID" + record.ID);
        SetSpriteByID(record.ID);
        tween = transform.DOScale(record.Scale, 0.25f);
        tween.OnComplete(()=>tween?.Kill());
    }
    public void RemoveCircle()
    {
        mergeVfx.GetComponent<ParticleSystem>().Play();
        CirclePool.instance.pool.DeSpawnNonGravity(this);
        
    }
    public void DropMergeStartCoroutine()
    {
        StartCoroutine(DropMergeCooldown());
    }
    public IEnumerator DropMergeCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        isDropping = false;
    }
    public void SetSpriteByID(int id)
    {
        var spriteName = EndlessLevel.Instance.GetSpriteName(id);
        spriteRenderer.sprite = SpriteLibControl.Instance.GetSpriteByName(spriteName);
    }
    public void SetDropVelocity()
    {
        fallSpeed = 10f;
        float currentVelocity = 0f;
        float newVelocity = Mathf.SmoothDamp(ridBody.velocity.y, fallSpeed, ref currentVelocity, smoothTime);

        // Apply the new velocity to the Rigidbody
        ridBody.velocity = new Vector2(ridBody.velocity.x, newVelocity);

    }
    public void SetColliderRadius()
    {
        record = ConfigFileManager.Instance.CircleConfig.GetRecordByKeySearch(TypeID);
        col.GetComponent<CircleCollider2D>().radius = record.Radius;
    }
    public void SetRigidBodyVelocity(Vector3 vl)
    {
        ridBody.velocity = vl;
    }
    public void SetRigidBodyToDynamic()
    {
        ridBody.bodyType = RigidbodyType2D.Dynamic;
    }
    public void SetAngularVelocity(float vl)
    {
        ridBody.angularVelocity = vl;
    }
    public void SetRigidBodyToKinematic()
    {
        ridBody.bodyType = RigidbodyType2D.Kinematic;
    }
}

