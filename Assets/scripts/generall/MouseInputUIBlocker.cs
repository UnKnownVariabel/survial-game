using UnityEngine;
using UnityEngine.EventSystems;

// MouseInputUIBlocker has the task to check if the mouse is blocked by the UI.
[RequireComponent(typeof(EventTrigger))]
public class MouseInputUIBlocker : MonoBehaviour
{
    public static bool BlockedByUI;
    private EventTrigger eventTrigger;

    // Start is called before the first frame update.
    private void Start()
    {
        eventTrigger = GetComponent<EventTrigger>();
        if (eventTrigger != null)
        {
            EventTrigger.Entry enterUIEntry = new EventTrigger.Entry();
            // Pointer Enter.
            enterUIEntry.eventID = EventTriggerType.PointerEnter;
            enterUIEntry.callback.AddListener((eventData) => { EnterUI(); });
            eventTrigger.triggers.Add(enterUIEntry);

            //Pointer Exit.
            EventTrigger.Entry exitUIEntry = new EventTrigger.Entry();
            exitUIEntry.eventID = EventTriggerType.PointerExit;
            exitUIEntry.callback.AddListener((eventData) => { ExitUI(); });
            eventTrigger.triggers.Add(exitUIEntry);
        }
    }

    // EnterUI sets BlockedByUI to true when mouse enters over UI.
    public void EnterUI()
    {
        BlockedByUI = true;
    }

    // EnterUI sets BlockedByUI to false when the mouse is no longer over UI.
    public void ExitUI()
    {
        BlockedByUI = false;
    }

}