using UnityEngine;
using ExplosionForce2D;
using UnityEngine.Events;
using ExplosionForce2D.PropertyAttributes;

[HelpURL("https://pulsarxstudio.com/Explosion%20Force%202D/")]
[AddComponentMenu("Physics 2D Toolkit/Double Explosion", order: 2)]
public class DoubleExplosion : UniversalExplosion
{

    private Collider2D[] colliders = new Collider2D[] { };

    [Header("First Explosion Settings")]
    [DrawRectWithColor(3f, 1f, 0f, 0f, 0.7f)]
    [Tooltip("Radius of the circle within witch the explosion has its effect.")]
    public float firstExplosionRadius = 2f;
    [Tooltip("The explosion position offset.")]
    public Vector3 firstExplosionOffset = default(Vector3);
    [Tooltip("The force of the explosion.")]
    public float firstExplosionForce = 30f;

    [Space(3)]
    [Tooltip("if set to true, Force will be modified by distance,NOTE! that bodies with the center of mass outside the radius will not be affected by the explosion!")]
    public bool firstExplosionModifyForceByDistance = true;
    [Tooltip("The method used to apply the force to its targets.")]
    public ForceMode2D firstExplosionForceMode = ForceMode2D.Impulse;


    [Header("Second Explosion Settings")]
    [DrawRectWithColor(3f, 0f, 0f, 1f, 0.7f)]
    [Tooltip("Delay of the second explosion (In seconds).")]
    public float secondExplosionDelay = 0.25f;
    [Space(6)]

    [Tooltip("Radius of the circle within witch the explosion has its effect.")]
    public float secondExplosionRadius = 3.5f;
    [Tooltip("The explosion position offset.")]
    public Vector3 secondExplosionOffset = default(Vector3);
    [Tooltip("The force of the explosion.")]
    public float secondExplosionForce = 50f;

    [Space(3)]
    [Tooltip("if set to true, Force will be modified by distance,NOTE! that bodies with the center of mass outside the radius will not be affected by the explosion!")]
    public bool secondExplosionModifyForceByDistance = true;
    [Tooltip("The method used to apply the force to its targets.")]
    public ForceMode2D secondExplosionForceMode = ForceMode2D.Impulse;


    [Space(15)]
    public UnityEvent onFirstExplosion = default(UnityEvent);
    [Space(5)]
    public UnityEvent onSecondExplosion = default(UnityEvent);

    private bool calculateTimers = false;
    private float secondExplosionDelayCounterTimer = 0f;

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
        calculateTimers = true;
        FirstExplosion();
    }

    private void FirstExplosion()
    {
        if (useNonAllocation)
        {
            FirstExplosionNonAlloc();
            return;
        }
        colliders = Physics2D.OverlapCircleAll(_transform.position + firstExplosionOffset, firstExplosionRadius, layerFilter, minDepth, maxDepth);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D Rb;
            if (useAttachedRigidbody)
                Rb = hit.attachedRigidbody;
            else
                Rb = hit.GetComponent<Rigidbody2D>();
            if (CheckCollider(hit) && CheckCollidedRigidbody2D(Rb))
            {
                Rb.AddExplosionForce2D(firstExplosionForce, _transform.position + firstExplosionOffset, firstExplosionRadius, firstExplosionModifyForceByDistance, firstExplosionForceMode);
                if (sendFirstExplosionDamage)
                    SendFirstExplosionDamage(Rb.gameObject);
            }
        }
        if (onFirstExplosion != null)
            onFirstExplosion.Invoke();
    }

    private void SecondExplosion()
    {
        if (useNonAllocation)
        {
            SecondExplosionNonAlloc();
            return;
        }
        colliders = Physics2D.OverlapCircleAll(_transform.position + secondExplosionOffset, secondExplosionRadius, layerFilter, minDepth, maxDepth);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D Rb;
            if (useAttachedRigidbody)
                Rb = hit.attachedRigidbody;
            else
                Rb = hit.GetComponent<Rigidbody2D>();
            if (CheckCollider(hit) && CheckCollidedRigidbody2D(Rb))
            {
                Rb.AddExplosionForce2D(secondExplosionForce, _transform.position + secondExplosionOffset, secondExplosionRadius, secondExplosionModifyForceByDistance, secondExplosionForceMode);
                if (sendSecondExplosionDamage)
                    SendSecondExplosionDamage(Rb.gameObject);
            }
        }
        if (onSecondExplosion != null)
            onSecondExplosion.Invoke();
        Finish();
    }

    protected override void UpdateCalculations()
    {
        if (calculateTimers)
        {
            secondExplosionDelayCounterTimer += Time.deltaTime;
            if (secondExplosionDelayCounterTimer >= secondExplosionDelay)
                SecondExplosion();
        }
    }

    protected override void Reset(bool deactivate = false)
    {
        calculateTimers = false;
        secondExplosionDelayCounterTimer = 0f;
        base.Reset(deactivate);
    }



    #region Advanced

    [Header("Advanced")]

    // First Explosion

    [Tooltip("Send the custom explosion damage.")]
    public bool sendFirstExplosionDamage = false;

    [Space(6)]

    [Tooltip("This is the custom damage and has nothing to do with explosion force. You can specify this, for example, based on enemy type.")]
    [BoolConditionalHide("sendFirstExplosionDamage", true, false)] public float firstExplosionDamage = 0f;

    [Tooltip("if set to true, 'Explosion Damage' will be modified based on the distance between rigidbodies and the explosion center.")]
    [BoolConditionalHide("sendFirstExplosionDamage", true, false)] public bool firstModifyDamageByDistance = true;

    [Space(6)]

    [Tooltip("The name of the method to call.")]
    [BoolConditionalHide("sendFirstExplosionDamage", true, false)] public string firstMethodToCall;

    [Tooltip("Should an error be raised if the method doesn't exist on the target object?")]
    [BoolConditionalHide("sendFirstExplosionDamage", true, false)] public SendMessageOptions firstOptions = SendMessageOptions.DontRequireReceiver;

    private float firstFinalExplosionDamage = 0f;
    private float firstDamageByDistanceModifier = 0f;

    private void SendFirstExplosionDamage(GameObject toObject)
    {
        if (firstModifyDamageByDistance)
        {
            firstDamageByDistanceModifier = 1.0f - (Vector2.Distance(_transform.position + firstExplosionOffset, toObject.transform.position) / firstExplosionRadius);
            if (firstDamageByDistanceModifier > 0f)
                firstFinalExplosionDamage = firstExplosionDamage * firstDamageByDistanceModifier;
            else
                return;
        }
        else
        {
            firstFinalExplosionDamage = firstExplosionDamage;
        }
        toObject.SendMessage(firstMethodToCall, firstFinalExplosionDamage, firstOptions);
    }


    // Second Explosion


    [Tooltip("Send the custom explosion damage.")]
    public bool sendSecondExplosionDamage = false;

    [Space(6)]

    [Tooltip("This is the custom damage and has nothing to do with explosion force. You can specify this, for example, based on enemy type.")]
    [BoolConditionalHide("sendSecondExplosionDamage", true, false)] public float secondExplosionDamage = 0f;

    [Tooltip("if set to true, 'Explosion Damage' will be modified based on the distance between rigidbodies and the explosion center.")]
    [BoolConditionalHide("sendSecondExplosionDamage", true, false)] public bool secondModifieDamageByDistance = true;

    [Space(6)]

    [Tooltip("The name of the method to call.")]
    [BoolConditionalHide("sendSecondExplosionDamage", true, false)] public string secondMethodToCall;

    [Tooltip("Should an error be raised if the method doesn't exist on the target object?")]
    [BoolConditionalHide("sendSecondExplosionDamage", true, false)] public SendMessageOptions secondOptions = SendMessageOptions.DontRequireReceiver;

    private float secondFinalExplosionDamage = 0f;
    private float secondDamageByDistanceModifier = 0f;

    private void SendSecondExplosionDamage(GameObject toObject)
    {
        if (secondModifieDamageByDistance)
        {
            secondDamageByDistanceModifier = 1.0f - (Vector2.Distance(_transform.position + secondExplosionOffset, toObject.transform.position) / secondExplosionRadius);
            if (secondDamageByDistanceModifier > 0f)
                secondFinalExplosionDamage = secondExplosionDamage * secondDamageByDistanceModifier;
            else
                return;
        }
        else
        {
            secondFinalExplosionDamage = secondExplosionDamage;
        }
        toObject.SendMessage(secondMethodToCall, secondFinalExplosionDamage, secondOptions);
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

    private void FirstExplosionNonAlloc()
    {

        // Then we make sure that we set all array values to null.
        for (int i = 0; i < collidersNonAlloc.Length; i++)
            collidersNonAlloc[i] = null;

        // Then we get all colliders that fall within circular area.
        Physics2D.OverlapCircleNonAlloc(_transform.position + firstExplosionOffset, firstExplosionRadius, collidersNonAlloc, layerFilter, minDepth, maxDepth);

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
                    Rb.AddExplosionForce2D(firstExplosionForce, _transform.position + firstExplosionOffset, firstExplosionRadius, firstExplosionModifyForceByDistance, firstExplosionForceMode); // We Add Explosion Force.
                    if (sendFirstExplosionDamage)
                        SendFirstExplosionDamage(Rb.gameObject);
                }
            }
        }

        if (onFirstExplosion != null)
            onFirstExplosion.Invoke(); // Invoke event.
    }

    // Second Explosion :
    // Make sure to comment the existing SecondExplosion() method!
    private void SecondExplosionNonAlloc()
    {

        // Then we make sure that we set all array values to null.
        for (int i = 0; i < collidersNonAlloc.Length; i++)
            collidersNonAlloc[i] = null;

        // Then we get all colliders that fall within circular area.
        Physics2D.OverlapCircleNonAlloc(_transform.position + secondExplosionOffset, secondExplosionRadius, collidersNonAlloc, layerFilter, minDepth, maxDepth);

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
                    Rb.AddExplosionForce2D(secondExplosionForce, _transform.position + secondExplosionOffset, secondExplosionRadius, secondExplosionModifyForceByDistance, secondExplosionForceMode); // We Add Explosion Force.
                    if (sendSecondExplosionDamage)
                        SendSecondExplosionDamage(Rb.gameObject);
                }
            }
        }

        if (onSecondExplosion != null)
            onSecondExplosion.Invoke(); // Invoke event.

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
