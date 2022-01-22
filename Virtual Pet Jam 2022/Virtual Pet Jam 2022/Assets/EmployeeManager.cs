using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum EmployeeEvent { None, Good, Bad }
public class EmployeeManager : MonoBehaviour
{
    Employee employee;
    EmployeeEvent currentEvent;
    [SerializeField] string overrideEmployeeName;
    [SerializeField] List<RuntimeAnimatorController> spriteAnimators;
    [SerializeField] RuntimeAnimatorController overrideSpriteAnimator;

    List<string> goodEvents = new List<string>() {
        " brings in Donuts.",
        " brings in Pizza.",
        " looks out the window and see's its a beautiful day!",
        " compliments another employee.",
        " hosts a pep rally to encourage the entire team!",
        " finds a way to be more productive around the office!",
        " closes an open ticket, and rings the 'Complete' gong!",
        " really loves this song as it is a banger!"
    };
    List<string> badEvents = new List<string>() {
        " breaks the Coffee maker!",
        " jam's the Printer!",
        " clicks a malicious link!",
        " spreads a rumor about another department!",
        " falls asleep at their desk while working!",
        " steals someone's little red stapler!"
    };

    [SerializeField] GameObject badEventEmoji;
    [SerializeField] GameObject goodEventEmoji;

    public void GenerateNewEmployeeEvent() {
        //Reset triggers
        GetComponent<Animator>().ResetTrigger("GoodEvent");
        GetComponent<Animator>().ResetTrigger("BadEvent");
        GetComponent<Animator>().ResetTrigger("NoEvent");

        //Good event or Bad event
        float eventCategory = Random.Range(-1f, 1f);
        if (eventCategory >= 0f) { //Good
            currentEvent = EmployeeEvent.Good;
            GetComponent<Animator>().SetTrigger("GoodEvent");
            goodEventEmoji.SetActive(true);
        } else { //Bad
            currentEvent = EmployeeEvent.Bad;
            GetComponent<Animator>().SetTrigger("BadEvent");
            badEventEmoji.SetActive(true);
        }
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ExecuteEmployeeEvent(TMP_Text eventDescription, Department activeDepartment) {
        if (currentEvent == EmployeeEvent.None) {
            eventDescription.text = "This is " + employee.GetName() + ". They " + employee.GetHobbie();
        } else {
            int pick; //Placeholder for what event string to use
            int stat = Random.Range(1, 3);

            //Random % mod
            float mod = Random.Range(0.01f, 0.1f);
            mod = Mathf.Round(mod * 100) / 100;
            string modString = (mod * 100).ToString("F0") + "%";

            //Random income mod
            int incomeMod = Random.Range(1, 5);
            switch (currentEvent) {
                case EmployeeEvent.Good:
                    pick = Random.Range(0, goodEvents.Count - 1);
                    if (stat == 1) {
                        activeDepartment.UpdateIncome(incomeMod);
                        eventDescription.text = employee.GetName() + goodEvents[pick] + " +" + incomeMod + " Income";
                    } else if (stat == 2) {
                        activeDepartment.UpdateMoralle(mod);
                        eventDescription.text = employee.GetName() + goodEvents[pick] + " +" + modString + " Moralle";
                    } else if (stat == 3) {
                        activeDepartment.UpdateProductivity(mod);
                        eventDescription.text = employee.GetName() + goodEvents[pick] + " +" + modString + " Productivity";
                    }
                    break;
                case EmployeeEvent.Bad:
                    pick = Random.Range(0, badEvents.Count - 1);
                    if (stat == 1) {
                        activeDepartment.UpdateIncome(incomeMod * -1);
                        eventDescription.text = employee.GetName() + badEvents[pick] + " -" + incomeMod + " Income";
                    } else if (stat == 2) {
                        activeDepartment.UpdateMoralle(mod * -1);
                        eventDescription.text = employee.GetName() + badEvents[pick] + " -" + modString + " Moralle";
                    } else if (stat == 3) {
                        activeDepartment.UpdateProductivity(mod * -1);
                        eventDescription.text = employee.GetName() + badEvents[pick] + " -" + modString + " Productivity";
                    }

                    break;
                default:
                    break;
            }

            DisableEmployeeEvent();
        }

        //Reset event description font
        eventDescription.color = Color.white;
        eventDescription.fontStyle = FontStyles.Normal;
    }

    public void DisableEmployeeEvent() {
        //Disable collider
        //GetComponent<BoxCollider2D>().enabled = false;

        //Change State
        currentEvent = EmployeeEvent.None;

        //Reset triggers
        GetComponent<Animator>().ResetTrigger("GoodEvent");
        GetComponent<Animator>().ResetTrigger("BadEvent");
        GetComponent<Animator>().ResetTrigger("NoEvent");

        //Trigger
        GetComponent<Animator>().SetTrigger("NoEvent");

        //Disable Emojis
        goodEventEmoji.SetActive(false);
        badEventEmoji.SetActive(false);
    }

    public void ActivateEmployee() {
        if(overrideEmployeeName != "") {
            employee = new Employee(overrideEmployeeName);
        } else {
            employee = new Employee();
        }

        if (overrideSpriteAnimator != null) {
            GetComponent<Animator>().runtimeAnimatorController = overrideSpriteAnimator;
        } else {
            int rng = Random.Range(0, spriteAnimators.Count - 1);
            GetComponent<Animator>().runtimeAnimatorController = spriteAnimators[rng];
        }
        gameObject.SetActive(true);
    }

    public string GetEmployeeName() {
        return employee.GetName();
    }

    bool checkForStateSwitch = false;
    private void Update() {
        //Hackary - Fix later
        if (checkForStateSwitch) {
            switch (currentEvent) {
                case EmployeeEvent.Bad:
                    GetComponent<Animator>().SetTrigger("BadEvent");
                    break;
                case EmployeeEvent.Good:
                    GetComponent<Animator>().SetTrigger("GoodEvent");
                    break;
                case EmployeeEvent.None:
                    GetComponent<Animator>().SetTrigger("NoEvent");
                    break;
                default:
                    print("State machine's brooken - Invalid state! " + currentEvent.ToString());
                    break;
            }
            checkForStateSwitch = false;
        }
    }
}
