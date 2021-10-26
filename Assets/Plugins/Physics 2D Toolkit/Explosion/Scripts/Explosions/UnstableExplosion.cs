using UnityEngine;
using ExplosionForce2D;
using UnityEngine.Events;
using ExplosionForce2D.PropertyAttributes;

[HelpURL("https://pulsarxstudio.com/Explosion%20Force%202D/")]
[AddComponentMenu("Physics 2D Toolkit/Unstable Explosion", order: 5)]
public class UnstableExplosion : UniversalExplosion
{

    private Collider2D[] colliders = new Collider2D[] { };

    [Header("Randomized Shaking Settings")]
    [DrawRectWithColor(3f, 1f, 0f, 0f, 0.7f)]
    [Tooltip("The frequency of shaking (In seconds)")]
    public float shakeFrequency = 0.05f;
    [Space(4)]
    [Tooltip("Radius of the circle within witch the shake has its effect.")]
    public float shakeRadius = 2.5f;
    [Tooltip("The Shake position offset.")]
    public Vector3 shakeOffset = default(Vector3);
    [Tooltip("The force of the shaking.")]
    public float shakeForce = 7f;

    [Space(3)]
    [Tooltip("if set to true, Force will be modified by distance,NOTE! that bodies with the center of mass outside the radius will not be affected by the explosion!")]
    public bool modifyShakingForceByDistance = true;
    [Tooltip("Whether the direction of force should be randomized ?")]
    public bool randomizeDirection = true;
    [Tooltip("Wheather the force should be multiplied with random value between 0 and 1 ?")]
    public bool randomizeForce = true;
    [Tooltip("The method used to apply the shake force to its targets.")]
    public ForceMode2D shakeForceMode = ForceMode2D.Impulse;



    [Space(1)]
    [Header("Explosion Settings")]
    [DrawRectWithColor(3f, 0f, 0f, 1f, 0.7f)]
    [Tooltip("Delay of the explosion (In seconds).")]
    public float explosionDelay = 0.6f;
    [Space(6)]

    [Tooltip("Radius of the circle within witch the attraction has its effect.")]
    public float explosionRadius = 4f;
    [Tooltip("The explosion position offset.")]
    public Vector3 explosionOffset = default(Vector3);
    [Tooltip("The force of the explosion.")]
    public float explosionForce = 80f;

    [Space(3)]
    [Tooltip("if set to true, Force will be modified by distance,NOTE! that bodies with the center of mass outside the radius will not be affected by the explosion!")]
    public bool modifyForceByDistance = true;
    [Tooltip("The method used to apply the force to its targets.")]
    public ForceMode2D forceMode = ForceMode2D.Impulse;

    [Space(15)]
    public UnityEvent onShake = default(UnityEvent);
    [Space(5)]
    public UnityEvent onExplosion = default(UnityEvent);

    private bool calculateTimers = false;
    private float timeSinceShakingStarted = 0f;
    private float shakeFrequencyCounter = 0f;

    void Awake()
    {
        UniversalAwake();
        if (useNonAllocation)
            collidersNonAlloc = new Collider2D[maxNumberOfColliders];
    }
    void OnEnable()
    {
        UniversalOnEnable();
    }
    void Update()
    {
        UnivesalUpdate();
    }

    public override void Activate()
    {
        base.Activate();
        Reset();
        Shake();
        calculateTimers = true;
    }

    private void Shake()
    {
        if (useNonAllocation)
        {
            ShakeNonAlloc();
            return;
        }
        colliders = Physics2D.OverlapCircleAll(_transform.position + shakeOffset, shakeRadius, layerFilter, minDepth, maxDepth);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D Rb;
            if (useAttachedRigidbody)
                Rb = hit.attachedRigidbody;
            else
                Rb = hit.GetComponent<Rigidbody2D>();
            if (CheckCollider(hit) && CheckCollidedRigidbody2D(Rb))
            {
                Rb.AddRandomizedExplosionForce2D(shakeForce, _transform.position + shakeOffset, shakeRadius, modifyShakingForceByDistance, randomizeDirection, randomizeForce, shakeForceMode);
                if (sendShakeDamage)
                    SendShakeDamage(Rb.gameObject);
            }
        }
        if (onShake != null)
            onShake.Invoke();
        shakeFrequencyCounter = 0f;
    }

    private void Explosion()
    {
        if (useNonAllocation)
        {
            ExplosionNonAlloc();
            return;
        }
        colliders = Physics2D.OverlapCircleAll(_transform.position + explosionOffset, explosionRadius, layerFilter, minDepth, maxDepth);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D Rb;
            if (useAttachedRigidbody)
                Rb = hit.attachedRigidbody;
            else
                Rb = hit.GetComponent<Rigidbody2D>();
            if (CheckCollider(hit) && CheckCollidedRigidbody2D(Rb))
            {
                Rb.AddExplosionForce2D(explosionForce, _transform.position + explosionOffset, explosionRadius, modifyForceByDistance, forceMode);
                if (sendExplosionDamage)
                    SendExplosionDamage(Rb.gameObject);
            }
        }
        if (onExplosion != null)
            onExplosion.Invoke();
        Finish();
    }

    protected override void Reset(bool deactivate = false)
    {
        calculateTimers = false;
        timeSinceShakingStarted = 0f;
        shakeFrequencyCounter = 0f;
        base.Reset(deactivate);
    }

    protected override void UpdateCalculations()
    {
        if (calculateTimers)
        {
            timeSinceShakingStarted += Time.deltaTime;
            if (timeSinceShakingStarted >= explosionDelay)
                Explosion();
            shakeFrequencyCounter += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (calculateTimers)
        {
            if (shakeFrequencyCounter >= shakeFrequency)
                Shake();
        }
    }

    #region Advanced

    [Header("Advanced")]

    // Explosion

    [Tooltip("Send the custom explosion damage.")]
    public bool sendExplosionDamage = false;

    [Space(6)]

    [Tooltip("This is the custom damage and has nothing to do with explosion force. You can specify this, for example, based on enemy type.")]
    [BoolConditionalHide("sendExplosionDamage", true, false)] public float explosionDamage = 0f;

    [Tooltip("if set to true, 'Explosion Damage' will be modified based on the distance between rigidbodies and the explosion center.")]
    [BoolConditionalHide("sendExplosionDamage", true, false)] public bool modifyDamageByDistance = true;

    [Space(6)]

    [Tooltip("The name of the method to call.")]
    [BoolConditionalHide("sendExplosionDamage", true, false)] public string methodToCall;

    [Tooltip("Should an error be raised if the method doesn't exist on the target object?")]
    [BoolConditionalHide("sendExplosionDamage", true, false)] public SendMessageOptions options = SendMessageOptions.DontRequireReceiver;

    private float finalExplosionDamage = 0f;
    private float damageByDistanceModifier = 0f;

    private void SendExplosionDamage(GameObject toObject)
    {
        if (modifyDamageByDistance)
        {
            damageByDistanceModifier = 1.0f - (Vector2.Distance(_transform.position + explosionOffset, toObject.transform.position) / explosionRadius);
            if (damageByDistanceModifier > 0f)
                finalExplosionDamage = explosionDamage * damageByDistanceModifier;
            else
                return;
        }
        else
        {
            finalExplosionDamage = explosionDamage;
        }
        toObject.SendMessage(methodToCall, finalExplosionDamage, options);
    }


    // Shake

    [Tooltip("Send the custom shake damage.")]
    public bool sendShakeDamage = false;

    [Space(6)]

    [Tooltip("This is the custom damage and has nothing to do with explosion force. You can specify this, for example, based on enemy type.")]
    [BoolConditionalHide("sendShakeDamage", true, false)] public float shakeDamage = 0f;

    [Tooltip("if set to true, 'Explosion Damage' will be modified based on the distance between rigidbodies and the explosion center.")]
    [BoolConditionalHide("sendShakeDamage", true, false)] public bool modifyShakeDamageByDistance = true;

    [Space(6)]

    [Tooltip("The name of the method to call.")]
    [BoolConditionalHide("sendShakeDamage", true, false)] public string shakeMethodToCall;

    [Tooltip("Should an error be raised if the method doesn't exist on the target object?")]
    [BoolConditionalHide("sendShakeDamage", true, false)] public SendMessageOptions shakeOptions = SendMessageOptions.DontRequireReceiver;

    private float finalEShakeDamage = 0f;
    private float shakeDamageByDistanceModifier = 0f;

    private void SendShakeDamage(GameObject toObject)
    {
        if (modifyShakeDamageByDistance)
        {
            shakeDamageByDistanceModifier = 1.0f - (Vector2.Distance(_transform.position + shakeOffset, toObject.transform.position) / shakeRadius);
            if (shakeDamageByDistanceModifier > 0f)
                finalEShakeDamage = shakeDamage * shakeDamageByDistanceModifier;
            else
                return;
        }
        else
        {
            finalEShakeDamage = shakeDamage;
        }
        toObject.SendMessage(shakeMethodToCall, finalEShakeDamage, shakeOptions);
    }

    #endregion


    #region OverlapCircleNonAlloc

    //  ----------------  Physics2D.OverlapCircleNonAlloc(); ----------------

    // By default all explosions use : Physics2D.OverlapCircleAll. And if you want to use : Physics2D.OverlapCircleNonAlloc. Here is example how to use it :
    // Disadvantage of using Physics2D.OverlapCircleNonAlloc is that you are limited on certian number of Rigidbodyes that can be affected with explosion, And
    // Advantage of using this is that no memory is allocated for the result, so garbage collection performance is improved when the check is pefrormed frequently.
    // See : https://docs.unity3d.com/ScriptReference/Physics2D.OverlapCircleNonAlloc.html

    [SerializeField] private bool useNonAllocation = false;
    [BoolConditionalHidePlusMessageBox("useNonAllocation", true, false, "Set the Max number of colliders you expect to be affected with explosion. \n" +
        "The significance of this is that no memory is allocated for the results and so garbage collection performance is improved.", "info")]
    [SerializeField] private int maxNumberOfColliders = 10;
    private Collider2D[] collidersNonAlloc = new Collider2D[10];

    private void ShakeNonAlloc()
    {

        // Then we make sure that we set all array values to null.
        for (int i = 0; i < collidersNonAlloc.Length; i++)
            collidersNonAlloc[i] = null;

        // Then we get all colliders that fall within circular area.
        Physics2D.OverlapCircleNonAlloc(_transform.position + shakeOffset, shakeRadius, collidersNonAlloc, layerFilter, minDepth, maxDepth);

        // For every Collider2D in our fixed array of Colllider2D
        foreach (Collider2D hit in collidersNonAlloc)
        {
            if (hit)
            { // We check if Collider2D exist (Is not null)
                Rigidbody2D Rb;
                if (useAttachedRigidbody)
                    Rb = hit.attachedRigidbody;
                else
                    Rb = hit.GetComponent<Rigidbody2D>();
                if (CheckCollider(hit) && CheckCollidedRigidbody2D(Rb))
                { // And if Rigidbody2D pass the filter check
                    Rb.AddRandomizedExplosionForce2D(shakeForce, _transform.position + shakeOffset, shakeRadius, modifyShakingForceByDistance, randomizeDirection, randomizeForce, shakeForceMode); // We Add Explosion Force.
                    if (sendShakeDamage)
                        SendShakeDamage(Rb.gameObject);
                }
            }
        }

        if (onShake != null)
            onShake.Invoke(); // Invoke event.
        shakeFrequencyCounter = 0f;
    }

    //Explosion :
    // Make sure to comment the existing Explosion() method!
    private void ExplosionNonAlloc()
    {

        // Then we make sure that we set all array values to null.
        for (int i = 0; i < collidersNonAlloc.Length; i++)
            collidersNonAlloc[i] = null;

        // Then we get all colliders that fall within circular area.
        Physics2D.OverlapCircleNonAlloc(_transform.position + explosionOffset, explosionRadius, collidersNonAlloc, layerFilter, minDepth, maxDepth);

        // For every Collider2D in our fixed array of Colllider2D
        foreach (Collider2D hit in collidersNonAlloc)
        {
            if (hit)
            { // We check if Collider2D exist (Is not null)
                Rigidbody2D Rb;
                if (useAttachedRigidbody)
                    Rb = hit.attachedRigidbody;
                else
                    Rb = hit.GetComponent<Rigidbody2D>();
                if (CheckCollider(hit) && CheckCollidedRigidbody2D(Rb))
                { // And if Rigidbody2D pass the filter check
                    Rb.AddExplosionForce2D(explosionForce, _transform.position + explosionOffset, explosionRadius, modifyForceByDistance, forceMode); // We Add Explosion Force.
                    if (sendExplosionDamage)
                        SendExplosionDamage(Rb.gameObject);
                }
            }
        }

        if (onExplosion != null)
            onExplosion.Invoke(); // Invoke event.
        Finish(); // Finish.
    }


    #endregion



#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (ExplosionForce2DPreferences.showGizoms && ExplosionForce2DPreferences.DoesExplosionGizmoExist())
            Gizmos.DrawIcon(transform.position, "ExplosionGizmo.png", true);
    }
#endif


}
