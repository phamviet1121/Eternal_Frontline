using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spam : MonoBehaviour
{
    public Spam_monsters spamMonsters;

    public List<Transform> locations;
    public List<GameObject> monsters;

    public int level;
    public int round;
    LevelData level0;

    private bool skipRoundDelay = false;

    private void Start()
    {
        level0 = spamMonsters.levels[level];
        round = 0;
        startrunning();
    }

    public void startrunning()
    {
        StartCoroutine(RunTurnsWithDelay());
    }

    public void SkipRoundDelay()
    {
        skipRoundDelay = true;
        Debug.Log("⏩ Yêu cầu bỏ qua chờ 30s round hiện tại!");
    }

    private void Update()
    {
        // Test bằng phím X
        if (Input.GetKeyDown(KeyCode.X))
        {
            SkipRoundDelay();
        }
    }

    IEnumerator RunTurnsWithDelay()
    {
        while (true) // Lặp vô hạn (tự reset round)
        {
            RoundData currentRound = level0.rounds[round];

            for (int turnIndex = 0; turnIndex < currentRound.turns.Count; turnIndex++)
            {
                TurndData turn = currentRound.turns[turnIndex];
                List<GroupData> groups = turn.groups;

                // In thông tin các Group
                for (int groupIndex = 0; groupIndex < groups.Count; groupIndex++)
                {
                    GroupData group = groups[groupIndex];
                    Debug.Log($"L{level}-R{round}-T{turnIndex}-G{groupIndex}: Loc={group.locations}, Qty={group.quantities}, Type={group.types}");
                }

                yield return new WaitForSeconds(2f); // Chờ sau khi in thông tin nhóm

                // In từng hàng sequentially
                int maxLength = 0;
                foreach (var group in groups)
                {
                    if (group.sequentially.Count > maxLength)
                        maxLength = group.sequentially.Count;
                }

                for (int i = 0; i < maxLength; i++)
                {
                    string line = $"→ ";

                    for (int groupIndex = 0; groupIndex < groups.Count; groupIndex++)
                    {
                        GroupData group = groups[groupIndex];
                        string groupHeader = $"G{groupIndex}(Loc:{group.locations}, Qty:{group.quantities}, Type:{group.types})";

                        if (i < group.sequentially.Count)
                            line += $"{groupHeader}[{i}] = {group.sequentially[i]}   ";
                        else
                            line += $"{groupHeader}[{i}] = ---   ";
                    }

                    Debug.Log(line);

                    // Tạo quái sau khi in
                    for (int groupIndex = 0; groupIndex < groups.Count; groupIndex++)
                    {
                        GroupData group = groups[groupIndex];

                        if (i < group.sequentially.Count)
                        {
                            int monsterIndex = group.sequentially[i] - 1;  // 👈 Trừ 1 để đúng index
                            int locationIndex = group.locations - 1;       // 👈 Trừ 1 để đúng index

                            if (monsterIndex >= 0 && monsterIndex < monsters.Count &&
                                locationIndex >= 0 && locationIndex < locations.Count)
                            {
                                GameObject prefab = monsters[monsterIndex];
                                Transform spawnPoint = locations[locationIndex];

                                GameObject spawned = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
                                spawned.name = $"Monster_L{level}_R{round}_T{i}_G{groupIndex}";
                            }
                            else
                            {
                                Debug.LogWarning($"⚠️ Không thể tạo monster: monsterIndex={group.sequentially[i]} (sau khi -1 là {monsterIndex}), " +
                                                 $"location={group.locations} (sau khi -1 là {locationIndex})");
                            }
                        }
                    }

                    yield return new WaitForSeconds(5f);
                }

                // ⏱ Chờ 10 giây sau khi hoàn thành 1 turn
                Debug.Log("⏳ Đợi 10 giây để chuyển sang turn tiếp theo...");
                yield return new WaitForSeconds(10f);
            }

            // ⏱ Chờ 30 giây sau khi hoàn thành 1 round (có thể bị bỏ qua nếu skipRoundDelay = true)
            Debug.Log($"✅ Hoàn thành ROUND {round}, chờ 30s để chuyển sang round mới...");

            float timer = 0f;
            float delay = 30f;
            while (timer < delay)
            {
                if (skipRoundDelay)
                {
                    Debug.Log("⏩ Đã bỏ qua chờ 30s, chuyển ngay sang round tiếp theo!");
                    skipRoundDelay = false;
                    break;
                }

                yield return null; // Chờ 1 frame
                timer += Time.deltaTime;
            }

            // Chuyển round, kiểm tra nếu vượt thì reset
            round++;
            if (round >= level0.rounds.Count)
            {
                Debug.Log("✅ Đã hoàn thành tất cả round! Reset round về 0...");
                yield break;
            }
        }
    }
}