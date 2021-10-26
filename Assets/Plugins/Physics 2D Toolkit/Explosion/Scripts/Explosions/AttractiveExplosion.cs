using UnityEngine;
using ExplosionForce2D;
using UnityEngine.Events;
using ExplosionForce2D.PropertyAttributes;

[HelpURL("https://pulsarxstudio.com/Explosion%20Force%202D/")]
[AddComponentMenu("Physics 2D Toolkit/Attractive Explosion", order: 4)]
public class AttractiveExplosion : UniversalExplosion
{

    private Collider2D[] colliders = new Collider2D[] { };

    [Header("Attraction Settings")]
    [DrawRectWithColor(3f, 1f, 0f, 0f, 0.7f)]
    [Tooltip("Radius of the circle within witch the attraction has its effect.")]
    public float attractionRadius = 3f;
    [Tooltip("The attraction position offset.")]
    public Vector3 attractionOffset = default(Vector3);
    [Tooltip("The force of the attraction.")]
    public float attractionForce = 40f;
    [Tooltip("Add or subtract the attraction force (Per Second). \nWhen the explosion is finished, the attraction Force will be reset to its starting value.")]
    public float forceOverTime = 0f;


    [Space(3)]
    [Tooltip("if set to true, Force will be modified by distance,NOTE! that bodies with the center of mass outside the radius will not be affected by the explosion!")]
    public bool modifyAttractionForceByDistance = true;
    [Tooltip("The method used to apply the attraction force to its targets.")]
    public ForceMode2D attractionForceMode = ForceMode2D.Impulse;

    [Space(1)]
    [Header("Explosion Settings")]
    [DrawRectWithColor(3f, 0f, 0f, 1f, 0.7f)]
    [Tooltip("Delay of the explosion (In seconds).")]
    public float explosionDelay = 0.4f;
    [Space(6)]

    [Tooltip("Radius of the circle within witch the explosion has its effect.")]
    public float explosionRadius = 4.5f;
    [Tooltip("The explosion position offset.")]
    public Vector3 explosionOffset = default(Vector3);
    [Tooltip("The force of the explosion.")]
    public float explosionForce = 70f;

    [Space(3)]
    [Tooltip("if set to true, Force will be modified by distance,NOTE! that bodies with the center of mass outside the radius will not be affected by the explosion!")]
    public bool modifyForceByDistance = true;
    [Tooltip("The method used to apply the force to its targets.")]
    public ForceMode2D forceMode = ForceMode2D.Impulse;

    [Space(15)]
    public UnityEvent onAttraction = default(UnityEvent);
    [Space(5)]
    public UnityEvent onExplosion = default(UnityEvent);

    private float capturedAttractionForce = 0f;
    private bool calculateTimers = false;
    private float explosionDelayCounterTimer = 0f;

    void Awake()
    {
        UniversalAwake();
        capturedAttractionForce = attractionForce;
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
        calculateTimers = true;
    }

    private void AttractionForceHandler()
    {
        if (useNonAllocation)
        {
            AttractionForceHandlerNonAlloc();
            return;
        }
        colliders = Physics2D.OverlapCircleAll(_transform.position + attractionOffset, attractionRadius, layerFilter, minDepth, maxDepth);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D Rb;
            if (useAttachedRigidbody)
                Rb = hit.attachedRigidbody;
            else
                Rb = hit.GetComponent<Rigidbody2D>();
            if (CheckCollider(hit) && CheckCollidedRigidbody2D(Rb))
            {
                Rb.AddAttractionForce2D(attractionForce, _transform.position + attractionOffset, attractionRadius, modifyAttractionForceByDistance, attractionForceMode);
                if (sendAttractionDamage)
                    SendAttractionDamage(Rb.gameObject);
            }
        }
        if (onAttraction != null)
            onAttraction.Invoke();
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

    protected override void UpdateCalculations()
    {
        if (calculateTimers)
        {
            explosionDelayCounterTimer += Time.deltaTime;
            if (explosionDelayCounterTimer >= explosionDelay)
                Explosion();

            if (forceOverTime > 0f)
            {
                attractionForce += forceOverTime * Time.deltaTime;
            }
            else if (forceOverTime < 0f)
            {
                attractionForce -= forceOverTime * Time.deltaTime;
            }
        }
    }

    void FixedUpdate()
    {
        if (calculateTimers)
            AttractionForceHandler();
    }

    protected override void Reset(bool deactivate = false)
    {
        calculateTimers = false;
        explosionDelayCounterTimer = 0f;
        attractionForce = capturedAttractionForce;
        base.Reset(deactivate);
    }

    #region Advanced

    [Header("Advanced")]

    // Explosion

    [Tooltip("Send the custom explosion damage.")]
    public bool sendExplosionDamage = false;

    [Space(6)]

    [Tooltip("This is the custom damage and has nothing to do with explosion force. You can specify this, for example, based on enemy type.")]
    [BoolConditionalHide("sendExplosionDamage", true, false)] public float explosionDamage = 0f;

    [Tooltip("if set to true, 'Explosion Damage' will be modified based on the distance between rigidbodies and the attraction center.")]
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

    // Attraction :

    [Tooltip("Send the custom explosion damage.")]
    public bool sendAttractionDamage = false;

    [Space(6)]

    [Tooltip("This is the custom damage and has nothing to do with explosion force. You can specify this, for example, based on enemy type.")]
    [BoolConditionalHide("sendAttractionDamage", true, false)] public float attractionDamage = 0f;

    [Tooltip("if set to true, 'Explosion Damage' will be modified based on the distance between rigidbodies and the attraction center.")]
    [BoolConditionalHide("sendAttractionDamage", true, false)] public bool modifieAttractionDamageByDistance = true;

    [Space(6)]

    [Tooltip("The name of the method to call.")]
    [BoolConditionalHide("sendAttractionDamage", true, false)] public string attractionMethodToCall;

    [Tooltip("Should an error be raised if the method doesn't exist on the target object?")]
    [BoolConditionalHide("sendAttractionDamage", true, false)] public SendMessageOptions attractionOptions = SendMessageOptions.DontRequireReceiver;

    private float finalAttractionDamage = 0f;
    private float attractionDamageByDistanceModifier = 0f;

    private void SendAttractionDamage(GameObject toObject)
    {
        if (modifieAttractionDamageByDistance)
        {
            attractionDamageByDistanceModifier = 1.0f - (Vector2.Distance(_transform.position + attractionOffset, toObject.transform.position) / attractionRadius);
            if (attractionDamageByDistanceModifier > 0f)
                finalAttractionDamage = attractionDamage * attractionDamageByDistanceModifier;
            else
                return;
        }
        else
        {
            finalAttractionDamage = attractionDamage;
        }
        toObject.SendMessage(attractionMethodToCall, finalAttractionDamage, attractionOptions);
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

    private void AttractionForceHandlerNonAlloc()
    {

        // Then we make sure that we set all array values to null.
        for (int i = 0; i < collidersNonAlloc.Length; i++)
            collidersNonAlloc[i] = null;

        // Then we get all colliders that fall within circular area.
        Physics2D.OverlapCircleNonAlloc(_transform.position + attractionOffset, attractionRadius, collidersNonAlloc, layerFilter, minDepth, maxDepth);

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
                    Rb.AddAttractionForce2D(attractionForce, _transform.position + attractionOffset, attractionRadius, modifyAttractionForceByDistance, attractionForceMode); // We Add Explosion Force.
                    if (sendAttractionDamage)
                        SendAttractionDamage(Rb.gameObject);
                }
            }
        }

        if (onAttraction != null)
            onAttraction.Invoke(); // Invoke event.
    }

    // Explosion :
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
