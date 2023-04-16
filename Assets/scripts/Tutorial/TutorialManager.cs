using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// TutorialManager manages tutorial tasks and checks if they are done.
public class TutorialManager : MonoBehaviour
{
    public int currentTask;

    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text nextText;
    [SerializeField] private TutorialTask[] tasks;
    [SerializeField] private float timeBetwenChecks;
    [SerializeField] private float warpTimeMultiplier;
    [SerializeField] private GameObject doneMenu;
    

    private bool _taskDone = false;

    // if task is done the next button becomes brighter.
    public bool taskDone
    {
        get
        {
            return _taskDone;
        }

        private set
        {
            _taskDone = value;
            if (value)
            {
                nextText.color = new Color(nextText.color.r, nextText.color.g, nextText.color.b, 1f);
            }
            else
            {
                nextText.color = new Color(nextText.color.r, nextText.color.g, nextText.color.b, 0.3f);
            }
        }
    }
    private bool notSpawnedMobs = true;
    private IEnumerator Coroutine;

    // Start is called before the first frame update.
    private void Start()
    {
        Coroutine = CheckTask();
        StartCoroutine(Coroutine);
        text.text = tasks[currentTask].text;
    }

    // Update is called once per frame.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Tab))
        {
            NextTask();
        }
    }

    // CheckTask waits for specified amount of seconds checks if task is done then calls itself again. 
    IEnumerator CheckTask()
    {
        yield return new WaitForSeconds(timeBetwenChecks);

        if (!(TimeHandler.instance.time > tasks[currentTask].minTime && TimeHandler.instance.time < tasks[currentTask].maxTime))
        {
            float multiplier = TimeHandler.instance.multiplier;
            TimeHandler.instance.multiplier = warpTimeMultiplier;

            if (TimeHandler.instance.time < tasks[currentTask].minTime)
            {
                yield return new WaitForSeconds((tasks[currentTask].minTime - TimeHandler.instance.time + 0.2f) * 3600 / warpTimeMultiplier);
            }
            else if (TimeHandler.instance.time > tasks[currentTask].maxTime)
            {
                yield return new WaitForSeconds((tasks[currentTask].minTime + 24 - TimeHandler.instance.time + 0.2f) * 3600 / warpTimeMultiplier);
            }
            else
            {
                Debug.Log("error");
            }
            TimeHandler.instance.multiplier = multiplier;
        }
        if (notSpawnedMobs)
        {
            for (int i = 0; i < tasks[currentTask].zombiesToSpawn; i++)
            {
                MobSpawner.instance.SpawnMob(0);
            }

            for (int i = 0; i < tasks[currentTask].wolfToSpawn; i++)
            {
                MobSpawner.instance.SpawnMob(1);
            }
            notSpawnedMobs = false;
        }

        if (Check())
        {
            taskDone = true;
        }


        if(!(currentTask >= tasks.Length))
        {
            Coroutine = CheckTask();
            StartCoroutine(Coroutine);
        }
        else
        {
            Debug.Log("checking task has ended");
            yield break;
        }

        bool Check()
        {
            List<Ingredient> ingredients = GetIngridients(tasks[currentTask].requiredItems);
            foreach (Ingredient ingredient in ingredients)
            {
                if (!Inventory.instance.CheckForIngredient(ingredient))
                {
                    return false;
                }
            }

            int[] neededBuildings = new int[8];
            int[] buildings = new int[8];

            for (int i = 0; i < tasks[currentTask].requiredBuildings.Length; i++)
            {
                neededBuildings[tasks[currentTask].requiredBuildings[i]]++;
            }

            foreach (Chunk chunk in Globals.chunks.Values)
            {
                for (int y = 0; y < WorldGeneration.chunkSize; y++)
                {
                    for (int x = 0; x < WorldGeneration.chunkSize; x++)
                    {
                        buildings[chunk.tiles[x, y] / 16]++;
                    }
                }
            }

            for (int i = 0; i < buildings.Length; i++)
            {
                if (neededBuildings[i] > buildings[i])
                {
                    return false;
                }
            }
            if (tasks[currentTask].zombiesToSpawn + tasks[currentTask].wolfToSpawn - Globals.mobs.Count < tasks[currentTask].mobQuota)
            {
                return false;
            }

            return true;

            List<Ingredient> GetIngridients(ItemData[] items)
            {
                List<Ingredient> ingredients = new List<Ingredient>();
                for (int i = 0; i < items.Length; i++)
                {
                    foreach (Ingredient ingredient in ingredients)
                    {
                        if (ingredient.item == items[i])
                        {
                            ingredient.amount += 1;
                            goto OuterLoop;
                        }
                    }
                    ingredients.Add(new Ingredient(items[i]));
                OuterLoop:
                    continue;
                }
                return ingredients;
            }
        }
    }

    // Advances to next task.
    public void NextTask()
    {
        if (taskDone)
        {
            currentTask++;
            if (currentTask >= tasks.Length)
            {
                doneMenu.SetActive(true);
                Globals.pause = true;
            }
            else
            {
                text.text = tasks[currentTask].text;
                notSpawnedMobs = true;
            }
            taskDone = false;
        }
    }
}
