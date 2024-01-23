using DG.Tweening;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CircleObject : FSMSystem
{
    [HideInInspector] public SpawnState Spawn;
    [HideInInspector] public DropState Drop;
    [HideInInspector] public MergeState Merge;
    [HideInInspector] public GroundedState Grounded;
    [HideInInspector] public DeadState Dead;
    [SerializeField] CircleTypeConfigRecord record;
    [SerializeField] private Rigidbody2D rigdBody;
    [SerializeField]
    private CircleCollider2D _collider;
    [SerializeField] private CircleObject contactCircle;
    [SerializeField] private int typeID;
    [SerializeField] private float instanceID;
    [SerializeField] private float fallSpeed;
    [SerializeField] float smoothTime;
    [SerializeField] private bool isDropping;
    [SerializeField] private bool isMerged;
    [SerializeField] private bool isBeingTarget;
    [SerializeField] private bool isSFXPlayed;
    [SerializeField] private bool isLanded;

    public Vector2 force;

    [SerializeField]
    private GameObject mergeVfx;
    [SerializeField]
    private TargetRender targetRender;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public int TypeID { get { return typeID; } }
    public bool IsMerged { get { return IsMerged; } }
    public bool IsDropping { get { return isDropping; } }
    public bool IsBeingTarget { get { return isBeingTarget; } }
    public bool IsSFXPlayed { get { return isSFXPlayed; } }
    public Rigidbody2D RigBody { get { return rigdBody; } }
    public CircleObject ContactCircle { get { return contactCircle; } }
    public TargetRender TargetRender { get { return targetRender; } }
    public void SetIsSFXPlayed(bool isPFXPlayed)
    {
        this.isSFXPlayed = isPFXPlayed;
    }
    public void SetIsDropping(bool isDropping)
    {
        this.isDropping = isDropping;
    }
    public void SetColliderRadius(float radius)
    {
        _collider.radius = radius;
    }
    public void SetIsMerge(bool isMerge)
    {
        this.isMerged = isMerge;
    }
    public void SetTypeID(int typeID)
    {
        this.typeID = typeID;
    }
    public void SetSpriteByID(int id)
    {
        var spriteName = SpriteLibControl.Instance.GetSpriteName(EndlessLevel.Instance.SpriteType, id);
        spriteRenderer.sprite = SpriteLibControl.Instance.GetSpriteByName(spriteName);
    }
    public void SetDropVelocity()
    {
        fallSpeed = 10f;
        float currentVelocity = 0f;
        float newVelocity = Mathf.SmoothDamp(rigdBody.velocity.y, fallSpeed, ref currentVelocity, smoothTime);

        // Apply the new velocity to the Rigidbody
        rigdBody.velocity = new Vector2(rigdBody.velocity.x, newVelocity);

    }
    public void SetColliderRadius()
    {
        record = ConfigFileManager.Instance.CircleConfig.GetRecordByKeySearch(TypeID);
        _collider.GetComponent<CircleCollider2D>().radius = record.Radius;
    }

    public void EnableTarget()
    {
        isBeingTarget = true;
        targetRender.EnableTarget();
    }
    public void DisableTarget()
    {
        isBeingTarget = false;
        targetRender.transform.DOScale(0f, 0.15f).OnComplete(() =>
        {
            targetRender.DisableTarget();
        });
    }
    public void SetRigidBodyVelocity(Vector3 vl)
    {
        rigdBody.velocity = vl;
    }
    public void SetRigidBodyToDynamic()
    {
        rigdBody.bodyType = RigidbodyType2D.Dynamic;
    }
    public void SetRigidBodyToNone()
    {
        rigdBody.constraints = RigidbodyConstraints2D.None;
        //Debug.Log(" SetRigidBodyToNone  " +rigdBody.constraints.ToString());
    }
    public void SetRigidBodyToFreeze()
    {
        rigdBody.constraints = RigidbodyConstraints2D.FreezePosition;
    }
    public void SetAngularVelocity(float vl)
    {
        rigdBody.angularVelocity = vl;
    }
    public void SetRigidBodyToKinematic()
    {
        rigdBody.bodyType = RigidbodyType2D.Kinematic;
    }
    private void Awake()
    {
        Spawn.Setup(this);
        Drop.Setup(this);
        Merge.Setup(this);
        Grounded.Setup(this);
        Dead.Setup(this);
        rigdBody = GetComponent<Rigidbody2D>();
        _collider = GetComponentInChildren<CircleCollider2D>();
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
    public void SetRotation(Vector3 rotate)
    {
        transform.rotation = new Quaternion(rotate.x, rotate.y, rotate.z, 0);
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MergeCircle") && isDropping == false && state != "SpawnState")
        {
            instanceID = Time.time;
            CircleObject otherCircle = collision.gameObject.GetComponentInParent<CircleObject>();
            contactCircle = otherCircle;
            SwitchCircleOption(otherCircle);
            PlayLandedSFX();
        } 
        else if (collision.gameObject.CompareTag("Wall") && isDropping == false && state != "SpawnState")
        {
            PlayLandedSFX();
            GotoState(Grounded);
            return;

        }

    }
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MergeCircle") && isDropping == false)
        {
            instanceID = Time.time;
            CircleObject otherCircle = collision.gameObject.GetComponentInParent<CircleObject>();

            if (isMerged || otherCircle.isMerged) return;
            contactCircle = otherCircle;
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
        yield return new WaitForSeconds(1f);
        isMerged = false;
    }
    public void ClaimPosition()
    {
        float x = transform.position.x;
        if (CameraMain.instance.main != null)
        {
            x = Mathf.Clamp(x, CameraMain.instance.GetLeft(), CameraMain.instance.GetRight());
            transform.position = new Vector3(x, transform.position.y);
        }
    }
    public virtual void SwitchCircleOption(CircleObject otherCircle)
    {

        if (typeID > 11)
        {
            otherCircle.RemoveCircle();
            RemoveCircle();
            return; //return when reach maximum
        }
        if (typeID != contactCircle.GetComponent<CircleObject>().typeID)
        {
            contactCircle = null;
            GotoState(Grounded);
            return;
        }
        else
        {

            if (instanceID < contactCircle.GetComponent<CircleObject>().instanceID) return;
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, GetComponentInChildren<CircleCollider2D>().radius);
        foreach (Collider2D col in hits)
        {
            if (col.gameObject != gameObject)
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
    public IEnumerator SpawnNewCircle(CircleObject col)
    {
        int t = typeID + 1;
        tween = col.transform.DOMove(transform.position, 0.2f);
        tween.OnComplete(() =>
        {
            RandomYaySFX(TypeID);
            col.RandomYaySFX(TypeID);
            EndlessLevel.Instance.RemoveCircle(col);
            EndlessLevel.Instance.RemoveCircle(this);
            tween?.Kill();
        });
        Physics2D.IgnoreCollision(_collider, col._collider);
        yield return new WaitForSeconds(0);
        col.GetComponent<CircleObject>().contactCircle = contactCircle = null;
        CirclePool.instance.pool.DeSpawnNonGravity(col.GetComponent<CircleObject>());
        CirclePool.instance.pool.DeSpawnNonGravity(this);
        if(typeID >10) yield  return null; 
        var c = CirclePool.instance.pool.SpawnNonGravityNext();
        c.SetTypeID(t);
        c.transform.localScale = Vector3.zero;
        c.SpawnCircle(t);

        c.record = ConfigFileManager.Instance.CircleConfig.GetRecordByKeySearch(c.typeID);
        c._collider.GetComponent<CircleCollider2D>().radius = c.record.Radius;
        PlayMergeVFX(c);//play spawn particles
        c.RandomMergeSFX();

        c.transform.SetPositionAndRotation(col.transform.position, Quaternion.identity);
        c.rigdBody.bodyType = RigidbodyType2D.Dynamic;
        PopAroundCircle();
        int score = typeID + c.typeID;
        IngameController.instance.AddScore(score);
        EndlessLevel.Instance.FindLargestType(typeID + 1);
    }
    public void PlayMergeVFX(CircleObject circle)
    {
        var vfx = circle.mergeVfx.GetComponent<ParticleSystem>();
        var color = circle.record.Color;
        SetParticleColor(color, vfx);
        vfx.Play();
    }
    void SetParticleColor(Color color, ParticleSystem particle)
    {
        ParticleSystem.MainModule mainModule = particle.main;
        mainModule.startColor= color;
    }
    public void SpawnCircle(int i)
    {
        instanceID = 0;
        record = ConfigFileManager.Instance.CircleConfig.GetRecordByKeySearch(i);
        DisableTarget();
        SetSpriteByID(record.ID);
        SetRigidBodyToNone();
        //spriteRenderer.gameObject.transform.DOScale(record.Scale, 0);
        EndlessLevel.Instance.AddCircle(this);
        isDropping = false;
        isLanded = false;
        tween = transform.DOScale(record.Scale, 0.25f);
        tween.OnComplete(() => tween?.Kill());
    }
    public void RemoveCircle()
    {
        mergeVfx.GetComponent<ParticleSystem>().Play();
        Reset();
        CirclePool.instance.pool.DeSpawnNonGravity(this);
    }
    public void DropMergeStartCoroutine()
    {
        if (gameObject.activeSelf == true) StartCoroutine(DropMergeCooldown());

    }
    public IEnumerator DropMergeCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        isDropping = false;
    }

    public void DeSpawnOnBomb(Action callback)
    {
        EndlessLevel.Instance.RemoveCircle(this);
        transform.DOScale(0, 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            gameObject.SetActive(false);
            CirclePool.instance.pool.DeSpawnNonGravity(this);
            callback?.Invoke();
        });

    }
    public void PlayLandedSFX()
    {
        if (!isLanded)
        {
            isLanded = !isLanded;
            SoundManager.Instance.PlaySFXWithVolume(SoundManager.SFX.LandedSFX,0.2f);
        }
    }

    public void RandomYaySFX(int value)
    {
        int positive = Random.Range(0,3);
        //Debug.Log("RandomYaySFX RATE" + positive);

        if (positive == 1 && isSFXPlayed == false)
        {
            if (value < 4)
            {
                SoundManager.Instance.PlaySFX(SoundManager.SFX.DropCircleSFX);
                SetIsSFXPlayed(true);
                return;
            }
        }
    }
    public void SetSpritePiority(int level)
    {
        if(level == 0)
        {
            spriteRenderer.sortingLayerName = "Default";
            //Debug.Log(spriteRenderer.sortingLayerName.ToString());
        }
        else if(level == 2)
        {
            spriteRenderer.sortingLayerName = "DropRender";
            //Debug.Log(spriteRenderer.sortingLayerName.ToString());
        }
    }
    public void RandomMergeSFX()
    {
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                SoundManager.Instance.PlaySFX(SoundManager.SFX.PopSFX);
                break;
            case 1:
                SoundManager.Instance.PlaySFX(SoundManager.SFX.PopSFX_2);
                break;
            case 2:
                SoundManager.Instance.PlaySFX(SoundManager.SFX.PopSFX_3);
                break;
            case 3:
                SoundManager.Instance.PlaySFX(SoundManager.SFX.PopSFX_4);
                break;
        }
    }
    public string GetCurrentState()
    {
        string crString = currentState.ToString();
        //Debug.Log("GetCurrentState " + crString);
        return crString;
    }
    private void Reset()
    {
        instanceID = 0;
        isBeingTarget = false;
        isDropping = false;
        isMerged = false;
        contactCircle = null;
        typeID = 0;
    }
    private void OnMouseDown()
    {
        if (IsBeingTarget)
        {
            if (EndlessLevel.Instance.IsBomb)
            {
                //Debug.Log("CLICKED ON BOMB");
                DeSpawnOnBomb(() =>
                {
                    EndlessLevel.Instance.AfterUsingBombItem();
                });
            }
            else if (EndlessLevel.Instance.IsUpgrade)
            {
                //Debug.Log("CLICKED ON UPGRADE" );
                typeID ++;
                SpawnCircle(TypeID + 1);
                EndlessLevel.Instance.AfterUpgradeItem();

            }
        }
    }
}

