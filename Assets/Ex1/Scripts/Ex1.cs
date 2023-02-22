using UnityEngine;

public class Ex1 : MonoBehaviour
{
    public UnoptimisedStruct1[] data;

    public Ex1Config config;


    // Start is called before the first frame update
    private void Start()
    {
        data = new UnoptimisedStruct1[config.size];
        for (var i = 0; i < config.size; i++)
        {
            var nbFriends = i % 10 + 1;
            var friends = new UnoptimizedStruct2[nbFriends];
            var nbObjectives = i % 40 + 1;
            for (var j = 0; j < nbFriends; j++)
            {
                friends[j] = new UnoptimizedStruct2(j % 4 != 0, Random.Range(0.2f, 2.0f), (FriendState)(j % 5), Random.Range(1, 5), j % nbObjectives, Random.Range(10, 50), j % 2 == 0, Random.Range(1, 2), Random.insideUnitSphere, Random.Range(5, 10));
            }
            var distancesFromObjectives = new float[nbObjectives];
            for (var j = 0; j < nbFriends; j++)
            {
                distancesFromObjectives[j] = Random.Range(100, 300);
            }
            var baseHp = Random.Range(100, 200);
            data[i] = new UnoptimisedStruct1(Random.Range(0, 4), i % 2 == 1, baseHp, nbFriends + Random.Range(4, 8), Random.insideUnitSphere, baseHp, distancesFromObjectives, (byte)Random.Range(0, 255), Random.Range(40, 80),
                new UnoptimizedStruct2(i % 4 != 0, Random.Range(0.2f, 2.0f), (FriendState)(i % 5), Random.Range(1, 5), i % nbObjectives, Random.Range(10, 50), i % 2 == 0, Random.Range(1, 2), Random.insideUnitSphere, Random.Range(5, 10)), i % 2 == 1, friends, i % 10 != 5, Random.Range(0.5f, 2.2f));
        }
    }
}
