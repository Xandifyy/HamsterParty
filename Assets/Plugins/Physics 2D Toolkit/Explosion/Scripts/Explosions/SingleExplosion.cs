using UnityEngine;
using ExplosionForce2D;
using UnityEngine.Events;
using ExplosionForce2D.PropertyAttributes;

[HelpURL("https://pulsarxstudio.com/Explosion%20Force%202D/")]
[AddComponentMenu("Physics 2D Toolkit/Single Explosion", order: 0)]
public class SingleExplosion : UniversalExplosion
{

    private Collider2D[] colliders = new Collider2D[] { };

    [Space(1)]
    [Tooltip("Activate the explosion when this Gameobject becomes disabled?")]
    public bool onDisable = false;

    [Space(2)]
    [Header("Explosion Settings")]
    [DrawRectWithColor(3f, 1f, 0f, 0f, 0.7f)]
    [Tooltip("Radius of the circle within witch the explosion has its effect.")]
    public float explosionRadius = 2.5f;
    [Tooltip("The explosion position offset.")]
    public Vector3 explosionOffset = default(Vector3);
    [Tooltip("The force of the explosion.")]
    public float explosionForce = 30f;

    [Space(3)]
    [Tooltip("if set to true, Force will be modified by distance,NOTE! that bodies with the center of mass outside the radius will not be affected by the explosion!")]
    public bool modifyForceByDistance = true;
    [Tooltip("The method used to apply the force to its targets.")]
    public ForceMode2D forceMode = ForceMode2D.Impulse;

    [Space(15)]
    public UnityEvent onExplosion = default(UnityEvent);

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
    void OnDisable()
    {
        OnDisableCheck(onDisable);
    }
    void Update()
    {
        UnivesalUpdate();
    }

    public override void Activate()
    {
        base.Activate();
        if (useNonAllocation)
        {
            ActivateNonAlloc();
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

    #region Advanced

    [Header("Advanced")]

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

    private void ActivateNonAlloc()
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
                //Rigidbody2D Rb = hit.GetComponent<Rigidbody2D>(); // Then try to get Rigidbody2D component from our Collider2D.
                Rigidbody2D Rb = hit.attachedRigidbody;
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