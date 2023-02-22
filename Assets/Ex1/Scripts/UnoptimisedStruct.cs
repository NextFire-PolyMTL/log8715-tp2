using UnityEngine;
public struct UnoptimisedStruct1
{
    public float velocity;
    public bool canJump;
    public int baseHP;
    public int nbAllies;
    public Vector3 position;
    public int currentHp;
    public float[] distancesFromObjectives;
    public byte colorAlpha;
    public double range;
    public UnoptimizedStruct2 mainFriend;
    public bool isVisible;
    public UnoptimizedStruct2[] otherFriends;
    public bool isStanding;
    public float size;
    
    public UnoptimisedStruct1(float velocity, bool canJump, int baseHP, int nbAllies, Vector3 position, int currentHp, float[] distancesFromObjectives, byte colorAlpha, double range, UnoptimizedStruct2 mainFriend, bool isVisible, UnoptimizedStruct2[] otherFriends, bool isStanding, float size)
    {
        this.velocity = velocity;
        this.canJump = canJump;
        this.baseHP = baseHP;
        this.nbAllies = nbAllies;
        this.position = position;
        this.currentHp = currentHp;
        this.distancesFromObjectives = distancesFromObjectives;
        this.colorAlpha = colorAlpha;
        this.range = range;
        this.mainFriend = mainFriend;
        this.isVisible = isVisible;
        this.otherFriends = otherFriends;
        this.isStanding = isStanding;
        this.size = size;
    }
}

public enum FriendState
{
    isFolowing,
    isSearching,
    isPatrolling,
    isGuarding,
    isJumping,
}

public struct UnoptimizedStruct2 
{
    public bool isAlive;
    public float height;
    public FriendState currentState;
    public float velocity;
    public int currentObjective;
    public double maxSight;
    public bool canJump;
    public float acceleration;
    public Vector3 position;
    public float maxVelocity;
    
    public UnoptimizedStruct2(bool isAlive, float height, FriendState currentState, float velocity, int currentObjective, double maxSight, bool canJump, float acceleration, Vector3 position, float maxVelocity)
    {
        this.isAlive = isAlive;
        this.height = height;
        this.currentState = currentState;
        this.velocity = velocity;
        this.currentObjective = currentObjective;
        this.maxSight = maxSight;
        this.canJump = canJump;
        this.acceleration = acceleration;
        this.position = position;
        this.maxVelocity = maxVelocity;
    }
}
