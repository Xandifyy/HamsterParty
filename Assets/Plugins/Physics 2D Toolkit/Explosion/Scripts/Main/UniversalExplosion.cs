using UnityEngine;
using System.Collections;
using ExplosionForce2D.PropertyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class UniversalExplosion : MonoBehaviour
{
    protected Rigidbody2D _Body;
    protected Transform _transform;

    #region FilterSettings

    [Header("Filter Settings")]
    [Space(2)]
    [Tooltip("Layers that will be under the influence of the explosion.")] // Layers that will be affected by the explosion.
    public LayerMask layerFilter = -1;
    [Space(1)]

    [Tooltip("Tags that will be under the influence of the explosion.")] // Tags that will be affected by the explosion.
    [TagDraw] public string[] tagFilter = new string[] { };

    [Space(3)]
    [Tooltip("Only include objects with a Z coordinate (depth) greater than or equal to this value.")]
    [BoolConditionalHide("useGamebjectDepth", false, true)] public float minDepth = -Mathf.Infinity;
    [Tooltip("Only include objects with a Z coordinate (depth) less than or equal to this value.")]
    [BoolConditionalHide("useGamebjectDepth", false, true)] public float maxDepth = Mathf.Infinity;
    [Space(1)]
    [Tooltip("Only include objects with a Z coordinate (depth) that are equal to this Gameobject Z coordinate (depth)")]
    public bool useGamebjectDepth = false;

    [Tooltip("Include Triggers colliders")]
    public bool useTriggerColliders = true;
    [Tooltip("Include Solid colliders")]
    [BoolConditionWarrningBox("useTriggerColliders", "Chose at least one type of collider!", false)]
    public bool useSolidColliders = true;
    [Tooltip("Get connected rigidbody attached to the same game object or attached to any parent game object. Otherwise explosion will work only if collider has Rigidbody attached to it self!")]
    public bool useAttachedRigidbody = true;

    #endregion

    #region FinishAction
    [Header("Finish Action")]

    [Tooltip("When the explosion is finished. Should this script automatically destroy itself?")]
    public bool destroyScript = false;
    [Space(1)]
    [Tooltip("When the explosion is finished. Should this Gameobject automatically destroy itself?")]
    public bool destroyGameobject = false;
    [Space(1)]
    [Tooltip("When the explosion is finished. Should this Gameobject automatically dissable itself?")]
    public bool deactivateGameobject = false;

    #endregion

    #region Activate
    [Header("Activate")]

    [Tooltip("Activate the explosion by the left mouse click ? \nThis is useful for testing.")]
    public bool onClick = false;
    [Space(1)]
    [Tooltip("Activate the explosion when this Gameobject becomes enabled?")]
    public bool onEnable = false;

    #endregion

    #region Universal

    /// <summary>
    /// Universal Update.
    /// </summary>
    protected void UnivesalUpdate()
    {
        UpdateCalculations();
        if (onClick)
        {
            if (Input.GetMouseButtonDown(0))
                Activate();
        }
    }
    /// <summary>
    /// Catches basic components.
    /// </summary>
    protected void UniversalAwake()
    {
        _Body = GetComponent<Rigidbody2D>();
        _transform = transform;
    }

    /// <summary>
    /// Checking wheather to occoure explosion when object is activated.
    /// </summary>
    protected void UniversalOnEnable()
    {
        if (onEnable)
            Activate();
    }

    /// <summary>
    /// Checking wheather to occoure explosion when object is deactivated.
    /// </summary>
    protected void OnDisableCheck(bool activateOnDisable)
    {
        if (activateOnDisable)
            Activate();
    }

    #endregion


    #region Main

    /// <summary>
    /// Checking if everything is fine before activating the explosion.
    /// </summary>
    private bool ActivateBaseCheck()
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning("GameObject is deactivated and you are trying to activate the Explosion.\n GameObject Name: " + gameObject.name + " - " + gameObject);
            return false;
        }
        return true;
    }


    /// <summary>
    /// Activates the explosion.
    /// </summary>
    public virtual void Activate()
    {
        if (!ActivateBaseCheck())
            return;
    }

    /// <summary>
    /// Updates the calculations.
    /// </summary>
    protected virtual void UpdateCalculations() { }

    /// <summary>
    /// Reset stats.
    /// </summary>
    protected virtual void Reset(bool deactivate = false)
    {
        if (deactivate)
            this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Checking wheather to destroy script, or Reset();
    /// </summary>
    protected virtual void Finish()
    {
        if (destroyGameobject)
        {
            Destroy(this.gameObject);
        }
        else if (destroyScript)
        {
            Destroy(this);
        }
        else if (deactivateGameobject)
        {
            Reset(true);
        }
        else
        {
            Reset();
        }
    }

    /// <summary>
    /// Checks the collided Rigidbody2D if it's compatible with filter settings.
    /// </summary>
    /// <returns><c>true</c>, if collided Rigidbody2D is compatible, <c>false</c> otherwise.</returns>
    /// <param name="_Rigidbody2D">Rigidbody2D to check.</param>
    protected bool CheckCollidedRigidbody2D(Rigidbody2D body)
    {
        if (body != null && !body.isKinematic)
        {

            if (tagFilter.Length != 0)
            {
                for (int i = 0; i < tagFilter.Length; i++)
                {
                    if (body.gameObject.tag == tagFilter[i])
                        return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks the collider if it's compatible with filter settings.
    /// </summary>
    /// <returns><c>true</c>, if collider was checked, <c>false</c> otherwise.</returns>
    /// <param name="coll">Coll.</param>
    protected bool CheckCollider(Collider2D coll)
    {
        if ((coll.isTrigger && useTriggerColliders) || (!coll.isTrigger && useSolidColliders))
            return true;
        return false;
    }

    #endregion
    
}
