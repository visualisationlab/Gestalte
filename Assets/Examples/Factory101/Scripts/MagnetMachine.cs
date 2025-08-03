using Examples.Factory101.Scripts;
using Examples.Factory101.Scripts.Input;
using Mediator;
using MoonSharp.Interpreter;
using UnityEngine;

public class MagnetMachine : Machine, IBuyable, IBlockPlacement, IHoverable, IPulseReceiver<McGibbleDescription>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float upgradePullMultiplier = 1.3f;
    public CircleCollider2D attractionArea;
    [SerializeField, Range(0f, 1f)] float pullFraction = 0.1f;
    [SerializeField] private float pullStrength = 2;
    [SerializeField] private float maxPullStrength = 2;
    [SerializeField] private SpriteRenderer effectCircle;

    public McGibbleDescription lastMcGibbleDetected;
    
    private void Start()
    {
        effectCircle.enabled = false;
    }
    
    protected override void RegisterLua()
    {
        UserData.RegisterType<MagnetMachine>();
        UserData.RegisterType<McGibbleDescription>();
        luaScript = new Script();
        luaScript.Globals["this"] = this;
    }

    protected override void AfterSetScript()
    {
        ExecuteScript();
    }

    public override string GetStatus()
    {
        string result = "";
        result += $"Level: {upgradeLevel}/{maxUpgradeLevel} \n";
        result += $"Magnet Strength: {GetPullStrength()}/{GetMaxPullStrength()} \n";
        return result;
    }

    public override void UpgradeMachine()
    {
        upgradeLevel++;
        
        pullFraction = upgradeLevel switch
        {
            2 => .3f,
            3 => .5f,
            _ => 1f // default case
        };
        
        maxPullStrength = upgradeLevel switch
        {
            2 => 5f,
            3 => 10f,
            _ => 1f // default case
        };
 
    }
    private void FixedUpdate()
    {
        Attract();
    }
    
    [ExposeMethod("Sets the strength of the magnet")]
    public void SetMagneticStrength(int strength)
    {
        pullStrength = Mathf.Clamp(strength, -1, maxPullStrength);
    }
    
    [ExposeMethod("Get the name of the item the connected sensor detected")]
    public string GetNameOfDetectedItem()
    {
        return lastMcGibbleDetected.name;
    }
    
    [ExposeMethod("Get the base sale price of the item the connected sensor detected")]
    public int GetBasePriceOfDetectedItem()
    {
        return lastMcGibbleDetected.raritySalePrice;
    }
    
    [ExposeMethod("Get the normalized rarity of the item the connected sensor detected")]
    public float GetRarityOfDetectedItem()
    {
        return lastMcGibbleDetected.normalizedRarity;
    }
    
    public void Attract()
    {
        if (attractionArea == null)
        {
            Debug.LogWarning("[MagnetMachine] attractionArea is not assigned.");
            return;
        }

        Vector2 magnetPos = transform.position;

        float scale = Mathf.Max(attractionArea.transform.lossyScale.x, attractionArea.transform.lossyScale.y);
        float worldRadius = attractionArea.radius * scale;

        Collider2D[] hits = Physics2D.OverlapCircleAll(magnetPos, worldRadius);

        foreach (var col in hits)
        {
            McGibble target = col.GetComponent<McGibble>();
            if (target == null)
                continue;

            Rigidbody2D rb = col.attachedRigidbody;
            if (rb == null)
                continue;

            Vector2 toMagnet = magnetPos - rb.position;
            float distance = toMagnet.magnitude;
            if (distance <= Mathf.Epsilon)
                continue;

            // Desired displacement this tick (same as before)
            float moveDistance = distance * pullFraction;

            // Target velocity to achieve that displacement in one FixedUpdate
            float fixedDt = Time.fixedDeltaTime;
            Vector2 desiredVelocity = toMagnet.normalized * (moveDistance / fixedDt);
    
            // Smoothly approach desired velocity
            rb.linearVelocity = desiredVelocity * (pullStrength * 0.25f);
        }
    }
    public void OnHoverEnter()
    {
        effectCircle.enabled = true;
    }

    public void OnHoverExit()
    {
        effectCircle.enabled = false;
    }
    
    private string GetPullStrength()
    {
        return pullStrength.ToString("0.0");
    }

    private string GetMaxPullStrength()
    {
        return maxPullStrength.ToString("0.0");
    }
    
    public int GetPrice()
    {
        return basePrice;
    }

    public string GetDescription()
    {
        return description;
    }

    public void OnPulse(McGibbleDescription message)
    {
        lastMcGibbleDetected = message;
        Debug.Log($"Received description: {message.name}");
    }
}
