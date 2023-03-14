using UnityEngine;
public struct UnoptimisedStruct1
{
    public UnoptimizedStruct2 mainFriend;  // 48 ??
    public UnoptimizedStruct2[] otherFriends; // Pointer = 8 Bytes
    public float[] distancesFromObjectives; // Pointer = 8 Bytes
    public double range; // Double = 8 Bytes
    public Vector3 position; // 3x float = 12 bytes
    public float velocity; // float = 4 Bytes
    public float size; // float = 4 Bytes
    public int baseHP; // int = 4 bytes
    public int nbAllies; // int = 4 bytes
    public int currentHp; // int = 4 bytes
    public byte colorAlpha; // Byte = 1 Byte
    public bool canJump; // Bool = 1 Byte
    public bool isVisible; // Bool = 1 Byte
    public bool isStanding; // Bool = 1 Byte

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
    public Vector3 position; // 3x float = 12 bytes
    public float height; // float = 4 Bytes
    public double maxSight; // Double = 8 Bytes
    public float velocity; // float = 4 Bytes
    public float acceleration; // float = 4 Bytes
    public float maxVelocity; // float = 4 Bytes
    public int currentObjective; // int = 4 Bytes
    public FriendState currentState; // enum short?? 1 Byte?
    public bool canJump; // Bool = 1 Byte
    public bool isAlive; // Bool = 1 Byte

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
