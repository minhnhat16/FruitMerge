using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CircleObject : FSMSystem
{
    [HideInInspector] public SpawnState Spawn;
    [HideInInspector] public DropState Drop;
    [HideInInspector] public MergeState Merge;
    [HideInInspector] public ShakeState Shake;
    [HideInInspector] public GroundedState Grounded;
    [HideInInspector] public DeadState Dead;
    [SerializeField] CircleType circleType;
    [SerializeField] private Rigidbody2D rigdBody;
    [SerializeField]
    private CircleCollider2D _collider;
    [SerializeField] private CircleObject contactCircle;
    [SerializeField] private int typeID;
    [SerializeField] private int skinType;
    [SerializeField] private int shakeDuration;
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
    private TargetRender targetRender;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public int TypeID { get { return typeID; } }
    public int ShakeDuration { get { return shakeDuration; } }

    public bool IsMerged { get { return IsMerged; } set { isMerged = value; } }
    public bool IsDropping { get { return isDropping; } set { isDropping = value; } }
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
        //Debug.Log("SpriteByID " + id);
        //id++;
        skinType = DataAPIController.instance.GetCurrentFruitSkin();
        var spriteName = SpriteLibControl.Instance.GetCircleSpriteName(skinType, id);
        //Debug.Log($"skinType {skinType}, id {id}, sprite name {spriteName}");
        spriteRenderer.sprite = SpriteLibControl.Instance.GetSpriteByName(spriteName);
    }
    public void SetDropVelocity()
    {
        fallSpeed *= Time.deltaTime;
        float currentVelocity = 0f;
        float newVelocity = Mathf.SmoothDamp(rigdBody.velocity.y, fallSpeed, ref currentVelocity, smoothTime);

        // Apply the new velocity to the Rigidbody
        rigdBody.velocity = new Vector2(rigdBody.velocity.x, newVelocity);

    }
    public void SetColliderRadius()
    {
        _collider.GetComponent<CircleCollider2D>().radius = circleType.Radius;
        //Debug.Log($" _collider.GetComponent<CircleCollider2D>().radius {circleType.Radius}");
    }

    public void EnableTarget()
    {
        isBeingTarget = true;
        if (state == "GroundedState")
        {
            targetRender.EnableTarget();
        }
    }
    public void DisableTarget()
    {
        isBeingTarget = false;
        Tween targetTween = targetRender.transform.DOScale(0f, 0.15f);
        targetTween.OnComplete(() =>
        {
            targetRender.DisableTarget();
            targetTween.Kill();
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
        Shake.Setup(this);
        Grounded.Setup(this);
        Dead.Setup(this);
        rigdBody = GetComponent<Rigidbody2D>();
        _collider = GetComponentInChildren<CircleCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetRotation(Vector3 rotate)
    {
        transform.rotation = new Quaternion(rotate.x, rotate.y, rotate.z, 0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionDetection(collision);
    }

    public void CollisionDetection(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MergeCircle") && isDropping == false)
        {
            instanceID = Time.time;
            isMerged = false;
            GotoState(Grounded);
            CircleObject otherCircle = collision.gameObject.GetComponentInParent<CircleObject>();
            if (otherCircle == null || IngameController.instance.isGameOver || otherCircle.isMerged || isMerged) return;
            contactCircle = otherCircle;
            SwitchCircleOption(otherCircle);
            return;
        }
        else if (collision.gameObject.CompareTag("Topwall") && transform.position.y > 8)
        {
            isMerged = false;
            IngameController.instance.isGameOver = true;
        }
      
    }
    public IEnumerator ResetMerge()
    {
        if(gameObject.activeInHierarchy)
        yield return new WaitForSeconds(1f);
        isMerged = false;
    }
    public void ClaimPosition()
    {
        float x = transform.position.x;
        if (CameraMain.instance.main == null && gameObject.activeSelf) return;
        else
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
        else if (typeID != otherCircle.GetComponent<CircleObject>().typeID)
        {
            contactCircle = null;
            GotoState(Grounded);
            return;
        }
        else
        {

            if (instanceID < otherCircle/*.GetComponent<CircleObject>()*/.instanceID || isDropping || otherCircle.isDropping) return;
            else
            {
                GotoState(Merge);
                isMerged = true;
                otherCircle.isMerged = true;
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
        yield return new WaitForSeconds(0.01f);
        col.GetComponent<CircleObject>().contactCircle = contactCircle = null;
        CirclePool.instance.pool.DeSpawnNonGravity(col.GetComponent<CircleObject>());
        CirclePool.instance.pool.DeSpawnNonGravity(this);

        if (typeID > 10) yield return null;

        var c = CirclePool.instance.pool.SpawnNonGravityNext();
        c.SetTypeID(t);
        c.transform.localScale = Vector3.zero;
        c.SpawnCircle(t);

        c._collider.radius = c.circleType.Radius;

       // PlayMergeVFX(c);//play spawn particles
        c.RandomMergeSFX();

        c.transform.SetPositionAndRotation(col.transform.position, Quaternion.identity);
        c.rigdBody.bodyType = RigidbodyType2D.Dynamic;
        c.ClaimPosition();
        PlayMergeVFX(c);
        //PopAroundCircle();
        IngameController.instance.AddScore(typeID + c.typeID);
        EndlessLevel.Instance.FindLargestType(typeID + 1);
    }
    public void PlayMergeVFX(CircleObject circle)
    {
        MergeVFX vfx = MergeVFXPool.instance.pool.SpawnNonGravity();
        vfx.SetTransform(transform.position);
        vfx.SetScale(transform.localScale + Vector3.one* 0.74f);
        var color = circle.circleType.Color;
        //Debug.Log($"PlayMergeVFX color {color}");
        SetParticleColor(color, vfx.MainVFX);
        vfx.PlayParticle();

    }
    void SetParticleColor(Color color, ParticleSystem particle)
    {
        ParticleSystem.MainModule mainModule = particle.main;
        mainModule.startColor = color;
    }
    public void SpawnCircle(int i)
    {
        //Debug.Log($"type id {i}");
        i--;
        if (IngameController.instance.isGameOver) return;
        instanceID = 0;
        DisableTarget();
        SetSpriteByID(i);
        SetRigidBodyToNone();
        EndlessLevel.Instance.AddCircle(this);
        isDropping = false;
        isLanded = false;
        circleType = ConfigFileManager.Instance.CircleConfig.GetRecordByKeySearch(skinType).GetTypeByID(i);
        //Debug.Log($"radius {circleType.Radius} + circleType.ID {circleType.ID}");
        tween = transform.DOScale(circleType.Scale, 0.25f);
        tween.OnComplete(() => tween?.Kill());
    }
    public void RemoveCircle()
    {
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
            SoundManager.Instance.PlaySFXWithVolume(SoundManager.SFX.LandedSFX, 0.2f);
        }
    }

    public void RandomYaySFX(int value)
    {
        int positive = Random.Range(0, 3);
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
        if (level == 0)
        {
            spriteRenderer.sortingLayerName = "Default";
            //Debug.Log(spriteRenderer.sortingLayerName.ToString());
        }
        else if (level == 2)
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
        if (currentState != null)
        {
            string crString = currentState.ToString();
            //Debug.Log("GetCurrentState " + crString);
            return crString;
        }
        return null;
    }
    public IEnumerator ForceCouroutine()
    {
        if (!gameObject.activeSelf) yield break;
        int elapseTime;
        int duration = shakeDuration;
        Debug.Log("ForceCourountine ");
        for (elapseTime = 0; elapseTime < duration; elapseTime++)
        {
            GotoState(Shake);
            float randomValue = Random.Range(150f, 250f);
            Debug.Log("randomValue " + randomValue);
            float x = Random.Range(-1, 1);
            float y = Random.Range(0, 1.5f);
            Vector3 force = 15f* randomValue * new Vector3(x,y);
            rigdBody.AddForce(force, ForceMode2D.Force);
            yield return new WaitForSeconds(0.75f);
        }   
    }
    public void ApplyForceOverTime()
    {
        if (gameObject.activeSelf == false) return;
        // Start the coroutine to gradually add force over time
        StartCoroutine(ForceCouroutine());
    }
    public void AddForceUp()
    {
        rigdBody.AddForce(2 * Vector2.up, ForceMode2D.Impulse);
    }
    public void AddForceLeft()
    {
        rigdBody.AddForce(Vector2.left, ForceMode2D.Impulse);

    }
    public void AddForceRight()
    {
        rigdBody.AddForce(Vector2.right, ForceMode2D.Impulse);

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
                typeID++;
                SpawnCircle(TypeID + 1);
                EndlessLevel.Instance.AfterUpgradeItem();

            }
        }
    }
}

