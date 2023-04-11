using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public int currentTask;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TutorialTask[] tasks;
    [SerializeField] private float timeBetwenChecks;
    [SerializeField] private float warpTimeMultiplier;
    [SerializeField] private GameObject doneMenu;

    private IEnumerator Coroutine;

    private void Start()
    {
        Coroutine = CheckTask();
        StartCoroutine(Coroutine);
        text.text = tasks[currentTask].text;
    }

    IEnumerator CheckTask()
    {
        if (Check())
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
                if (!(Globals.timeHandler.time > tasks[currentTask].minTime && Globals.timeHandler.time < tasks[currentTask].maxTime))
                {
                    float multiplier = Globals.timeHandler.multiplier;
                    Globals.timeHandler.multiplier = warpTimeMultiplier;

                    if (Globals.timeHandler.time < tasks[currentTask].minTime)
                    {
                        yield return new WaitForSeconds((tasks[currentTask].minTime - Globals.timeHandler.time + 0.2f) * 3600 / warpTimeMultiplier);
                    }
                    else if (Globals.timeHandler.time > tasks[currentTask].maxTime)
                    {
                        yield return new WaitForSeconds((tasks[currentTask].minTime + 24 - Globals.timeHandler.time + 0.2f) * 3600 / warpTimeMultiplier);
                    }
                    else
                    {
                        Debug.Log("error");
                    }
                    Globals.timeHandler.multiplier = multiplier;
                }

                for (int i = 0; i < tasks[currentTask].zombiesToSpawn; i++)
                {
                    MobSpawner.instance.SpawnMob(0);
                }

                for (int i = 0; i < tasks[currentTask].wolfToSpawn; i++)
                {
                    MobSpawner.instance.SpawnMob(1);
                }
            }
            
        }

        yield return new WaitForSeconds(timeBetwenChecks);

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
}
