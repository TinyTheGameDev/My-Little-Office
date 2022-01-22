using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    string businessType = "Law Firm";
    float timer;
    int totalMoney;
    //[SerializeField] List<GameObject> employees;

    [SerializeField] List<GameObject> departmentZeroEmployeeSpots;
    [SerializeField] List<GameObject> departmentOneEmployeeSpots;
    [SerializeField] List<GameObject> departmentTwoEmployeeSpots;
    [SerializeField] List<GameObject> departmentThreeEmployeeSpots;
    [SerializeField] List<GameObject> departmentFourEmployeeSpots;
    float employeeEventTimer;

    [Header("UI")]
    [SerializeField] TMP_Text income_UI;
    [SerializeField] TMP_Text productivity_UI;
    [SerializeField] TMP_Text moralle_UI;
    [SerializeField] TMP_Text totalMoney_UI;
    [SerializeField] TMP_Text eventDescription_UI;
    [SerializeField] TMP_Text departmentName_UI;

    [Header("Actions")]
    [SerializeField] Button offerBonusIncentiveButton;
    [SerializeField] Button buyDepartmentLunchButton;
    [SerializeField] Button microManageButton;
    [SerializeField] Button hireEmployeeButton;
    [SerializeField] Button temp_MoraleButton;
    [SerializeField] Button temp_ProductivityButton;

    float offerBonusIncentiveTimer;
    float buyDepartmentLunchTimer;
    float microManageTimer;
    float TEMP_MoraleTimer;
    float TEMP_ProductivityTimer;
    float hireEmployeeTimer;

    Department reception = new Department("Reception", 1, 1, 1, new Vector3(-15f, 0f, -10f));
    Department collections = new Department("Collections", 1, 1, 1, new Vector3(0f, 0f, -10f));
    Department humanResources = new Department("Human Resources", 1, 1, 1, new Vector3(15f, 0f, -10f));
    Department accounting = new Department("Accounting", 1, 1, 1, new Vector3(30f, 0f, -10f));
    Department informationTechnology = new Department("IT", 1, 1, 1, new Vector3(45f, 0f, -10f));
    List<Department> departments = new List<Department>();
    Department activeDepartment;
    // Start is called before the first frame update
    void Start() {
        //Set up departments
        departments.Add(reception);
        departmentZeroEmployeeSpots[0].GetComponent<EmployeeManager>().ActivateEmployee();
        reception.AddEmployee(departmentZeroEmployeeSpots[0]);

        departments.Add(collections);
        departmentOneEmployeeSpots[0].GetComponent<EmployeeManager>().ActivateEmployee();
        collections.AddEmployee(departmentOneEmployeeSpots[0]);

        departments.Add(humanResources);
        departmentTwoEmployeeSpots[0].GetComponent<EmployeeManager>().ActivateEmployee();
        humanResources.AddEmployee(departmentTwoEmployeeSpots[0]);

        departments.Add(accounting);
        departmentThreeEmployeeSpots[0].GetComponent<EmployeeManager>().ActivateEmployee();
        accounting.AddEmployee(departmentThreeEmployeeSpots[0]);

        departments.Add(informationTechnology);
        departmentFourEmployeeSpots[0].GetComponent<EmployeeManager>().ActivateEmployee();
        informationTechnology.AddEmployee(departmentFourEmployeeSpots[0]);

        activeDepartment = reception;

        eventDescription_UI.text = "TUTORIAL: Manage departments by clicking on buttons at the bottom. Interact with employees by clicking on them. Click the arrow buttons to change departments.";

        employeeEventTimer = 30f;
        StartCoroutine("HandleTicks");
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);
            if(hit.collider != null) {
                hit.collider.GetComponent<EmployeeManager>().ExecuteEmployeeEvent(eventDescription_UI, activeDepartment);
            }
        }

        RefreshUI();
    }

    //TODO: If department moralle is at 0, the department lights on fire and a notification is sent out
    IEnumerator HandleTicks() {
        for(; ; ) {
            //TODO: Check for win/loose condition?
            HandleRandomMoralleLoss();
            HandleEmployeeEvent();
            HandleReceptionCalculations();
            HandleCollectionsCalculations();
            HandleAccountingCalculations();
            HandleHumanResourcesCalculations();
            HandleInformationTechnologyCalculations();
            HandleCooldowns();
            //RefreshUI();
            totalMoney_UI.text = "Total Money" + "\n" + "$" + totalMoney;
            yield return new WaitForSeconds(1f);
        }
    }

    //Handle Department Calculations
    #region Department Calculations
    void HandleRandomMoralleLoss() {
        int d = Random.Range(0, departments.Count);
        departments[d].UpdateMoralle(-.01f);
    }

    void HandleReceptionCalculations() {
        //Update Total Money
        totalMoney += reception.GetIncome();
    }
    void HandleCollectionsCalculations() {
        //Update total Money
        totalMoney += collections.GetIncome();
    }

    void HandleHumanResourcesCalculations() {
        //Update total Money
        totalMoney += humanResources.GetIncome();
    }

    void HandleAccountingCalculations() {
        //Update total Money
        totalMoney += accounting.GetIncome();
    }

    void HandleInformationTechnologyCalculations() {
        //Update total Money
        totalMoney += informationTechnology.GetIncome();
    }
    #endregion Department Calculations

    void RefreshUI() {
        //Update UI
        departmentName_UI.text = activeDepartment.GetDepartmentName();// + " Department";
        income_UI.text = "Income" + "\n" + "$" + activeDepartment.GetIncome();
        moralle_UI.text = "Morale" + "\n" + activeDepartment.GetMoralleString();
        productivity_UI.text = "Productivity" + "\n" + activeDepartment.GetProductivityString();

        //Ability 1 UI Refresh
        if (offerBonusIncentiveTimer <= 0f) {
            offerBonusIncentiveButton.GetComponentInChildren<TMP_Text>().text = "Offer Bonus Incentive" + "\n" + "-$100";
        } else {
            offerBonusIncentiveButton.GetComponentInChildren<TMP_Text>().text = "Offer Bonus Incentive" + "\n" + "CD: " + offerBonusIncentiveTimer.ToString();
        }

        //Ability 2 UI Refresh
        if (buyDepartmentLunchTimer <= 0f) {
            buyDepartmentLunchButton.GetComponentInChildren<TMP_Text>().text = "Buy Dept. Lunch" + "\n" + " -$" + CalculateLunchPrice();
        } else {
            buyDepartmentLunchButton.GetComponentInChildren<TMP_Text>().text = "Buy Dept. Lunch" + "\n" + "CD: " + buyDepartmentLunchTimer.ToString();
        }

        //Ability 3 UI Refresh
        if (microManageTimer <= 0f) {
            microManageButton.GetComponentInChildren<TMP_Text>().text = "Micro Manage" + "\n" + " -25% morale ";
        } else {
            microManageButton.GetComponentInChildren<TMP_Text>().text = "Micro Manage" + "\n" + "CD: " + microManageTimer.ToString();
        }

        //Ability 4 UI Refresh
        if (hireEmployeeTimer <= 0f) {
            if (departments.IndexOf(activeDepartment) == 0 || departments[departments.IndexOf(activeDepartment)].IsDepartmentFull()) {
                hireEmployeeButton.GetComponentInChildren<TMP_Text>().text = "No positions available";
            } else {
                hireEmployeeButton.GetComponentInChildren<TMP_Text>().text = "Hire New Employee" + "\n" + "-$" + CalculateHirePrice();
            }
        } else {
            if (departments.IndexOf(activeDepartment) == 0 || departments[departments.IndexOf(activeDepartment)].IsDepartmentFull()) {
                hireEmployeeButton.GetComponentInChildren<TMP_Text>().text = "No positions available";
            } else {
                hireEmployeeButton.GetComponentInChildren<TMP_Text>().text = "Hire New Employee" + "\n" + "CD: " + hireEmployeeTimer.ToString();
            }
        }

        //Ability 5 UI Refresh
        if(TEMP_MoraleTimer <= 0f) {
            temp_MoraleButton.GetComponentInChildren<TMP_Text>().text = "Give Kudos" + "\n" + "+15% Morale";
        } else {
            temp_MoraleButton.GetComponentInChildren<TMP_Text>().text = "Give Kudos" + "\n" + "CD: " + TEMP_MoraleTimer.ToString();
        }

        //TODO: Ability 6 UI Refresh
        if (TEMP_ProductivityTimer <= 0f) {
            temp_ProductivityButton.GetComponentInChildren<TMP_Text>().text = "Upgrade PC" + "\n" + "+15% Prod.";
        } else {
            temp_ProductivityButton.GetComponentInChildren<TMP_Text>().text = "Upgrade PC" + "\n" + " CD: " + TEMP_ProductivityTimer.ToString();
        }
    }

    public void ChangeDepartment(string direction) {
        //Change Active Department Enum
        int pos = departments.IndexOf(activeDepartment);
        switch(direction) {
            case "Left":
                pos--;
                if(pos < 0) {
                    pos = departments.Count - 1;
                }
                break;
            case "Right":
                pos++;
                if(pos > departments.Count - 1) {
                    pos = 0;
                }
                break;
            default:
                break;
        }
        activeDepartment = departments[pos];

        //UI fixes
        if (eventDescription_UI.color != Color.red) {
            eventDescription_UI.text = "Lets see what the " + activeDepartment.GetDepartmentName() + " Department is up to...";
        }
        if(eventDescription_UI.fontSize != 16) { eventDescription_UI.fontSize = 16; }

        //Move camera
        Camera.main.GetComponent<CameraManager>().ChangeCameraPosition(activeDepartment.GetDepartmentPosition());

        //Update UI
        RefreshUI();
    }

    void HandleEmployeeEvent() {
        if(employeeEventTimer > 0f) {
            employeeEventTimer -= 1f;
        } else {
            //Reset timer
            employeeEventTimer = Random.Range(20f, 30f);

            //Deactivate previous event
            foreach(Department dept in departments) {
                foreach(GameObject emp in dept.GetEmployees()) {
                    emp.GetComponent<EmployeeManager>().DisableEmployeeEvent();
                }
            }

            //Pick random department. Ignore 0, because no drama ever happens in reception xD
            int departmentPick = Random.Range(1, departments.Count - 1);

            //Activate a random employee
            departments[departmentPick].GetRandomEmployee().GetComponent<EmployeeManager>().GenerateNewEmployeeEvent();

            //Trigger player notification
            if (eventDescription_UI.fontSize != 16) { eventDescription_UI.fontSize = 16; }
            eventDescription_UI.text = "Somethings going on in the " + departments[departmentPick].GetDepartmentName() + " Department";
            eventDescription_UI.color = Color.red;
            eventDescription_UI.fontStyle = FontStyles.Bold;
        }
    }

    //Abilitys
    #region Abilitys
    void HandleCooldowns() {
        //Ability 1
        if (offerBonusIncentiveTimer > 0f) {
            offerBonusIncentiveTimer -= 1f;
        }

        //Ability 2
        if (buyDepartmentLunchTimer > 0f) {
            buyDepartmentLunchTimer -= 1f;
        }

        //Ability 3
        if (microManageTimer > 0f) {
            microManageTimer -= 1f;
        }

        //Ability 4
        if(hireEmployeeTimer > 0f) {
            hireEmployeeTimer -= 1f;
        }

        //Ability 5
        if (TEMP_MoraleTimer > 0f) {
            TEMP_MoraleTimer -= 1f;
        }

        //Ability 6
        if (TEMP_ProductivityTimer > 0f) {
            TEMP_ProductivityTimer -= 1f;
        }

    }

    public void OfferBonusIncentive() { //Offer the department a bonus - Productivity, income and moralle increases
        double productivityMod = .05;
        int incomeMod = 5;
        double moralleMod = .05;
        if (totalMoney >= 100) {
            totalMoney -= 100;
        
            if (offerBonusIncentiveTimer <= 0) {
                offerBonusIncentiveTimer = 30f;

                activeDepartment.UpdateProductivity(productivityMod);
                activeDepartment.UpdateIncome(incomeMod);
                activeDepartment.UpdateMoralle(moralleMod);

                if (eventDescription_UI.fontSize != 16) { eventDescription_UI.fontSize = 16; }
                eventDescription_UI.text = "You offer " + activeDepartment.GetDepartmentName() + " a bonus! The department is performing much better with the added motivation!";
            }
        }
    }

    public void BuyDepartmentLunch() { //Buy the department lunch - Productivity decreases but moralle increases
        double productivityMod = -.05;
        int incomeMod = 0;
        double moralleMod = .25;

        if (totalMoney >= CalculateLunchPrice()) {
            totalMoney -= CalculateLunchPrice();

            if (buyDepartmentLunchTimer <= 0) {
                buyDepartmentLunchTimer = 10f;
                activeDepartment.UpdateProductivity(productivityMod);
                activeDepartment.UpdateIncome(incomeMod);
                activeDepartment.UpdateMoralle(moralleMod);

                if (eventDescription_UI.fontSize != 16) { eventDescription_UI.fontSize = 16; }
                eventDescription_UI.text = "You buy the department lunch! During the long lunch less work is done but moralle is much higher!";
            }
        }
    }

    int CalculateLunchPrice() {
        return (activeDepartment.GetDepartmentSize() * 10);
    }

    public void MicroManage() { //Micro Manage department - Productivity and Income go up, but Moralle decreases
        double productivityMod = .25;
        int incomeMod = 10;
        double moralleMod = -.25;
        if (microManageTimer <= 0f) {
            microManageTimer = 10f;
            activeDepartment.UpdateProductivity(productivityMod);
            activeDepartment.UpdateIncome(incomeMod);
            activeDepartment.UpdateMoralle(moralleMod);

            if (eventDescription_UI.fontSize != 16) { eventDescription_UI.fontSize = 16; }
            eventDescription_UI.text = "You Micro Manage the department, Nobody is excited about being watched this closely but the department is performing better overall.";
        }
    }

    public void HireNewEmployee() {
        if (hireEmployeeTimer <= 0f) {
            if (totalMoney >= CalculateHirePrice()) { //If player can offord, try to add
                hireEmployeeTimer = 5f;
                int pos = departments.IndexOf(activeDepartment);
                int size = activeDepartment.GetDepartmentSize();
                if (!departments[pos].IsDepartmentFull()) { //If active department is not full, add employee to department
                                                            //Take cost of employee
                    totalMoney -= CalculateHirePrice();

                    //Add and activate employee
                    if (pos == 1) {
                        departmentOneEmployeeSpots[size].GetComponent<EmployeeManager>().ActivateEmployee();
                        activeDepartment.AddEmployee(departmentOneEmployeeSpots[size]);
                    } else if (pos == 2) {
                        departmentTwoEmployeeSpots[size].GetComponent<EmployeeManager>().ActivateEmployee();
                        activeDepartment.AddEmployee(departmentTwoEmployeeSpots[size]);
                    } else if (pos == 3) {
                        departmentThreeEmployeeSpots[size].GetComponent<EmployeeManager>().ActivateEmployee();
                        activeDepartment.AddEmployee(departmentThreeEmployeeSpots[size]);
                    } else if (pos == 4) {
                        departmentFourEmployeeSpots[size].GetComponent<EmployeeManager>().ActivateEmployee();
                        activeDepartment.AddEmployee(departmentFourEmployeeSpots[size]);
                    }
                }

                //Increase income per employee
                activeDepartment.UpdateIncome(1);
            }
        }
    }

    public void TEMP_MoraleAbility() {
        if (totalMoney >= 0) { //If player can offord
            double productivityMod = 0;
            int incomeMod = 0;
            double moralleMod = .15;
            totalMoney -= 0;
            if (TEMP_MoraleTimer <= 0f) {
                TEMP_MoraleTimer = 5f;
                activeDepartment.UpdateProductivity(productivityMod);
                activeDepartment.UpdateIncome(incomeMod);
                activeDepartment.UpdateMoralle(moralleMod);

                if (eventDescription_UI.fontSize != 16) { eventDescription_UI.fontSize = 16; }
                eventDescription_UI.text = "You give kudos to " + activeDepartment.GetRandomEmployeeName() + ". Their morale increases!";
            }
        }
    }

    public void TEMP_ProductivityAbility() {
        if (totalMoney >= 0) { //If player can offord
            double productivityMod = .15;
            int incomeMod = 0;
            double moralleMod = 0;
            totalMoney -= 1;
            if (TEMP_ProductivityTimer <= 0f) {
                TEMP_ProductivityTimer = 5f;
                activeDepartment.UpdateProductivity(productivityMod);
                activeDepartment.UpdateIncome(incomeMod);
                activeDepartment.UpdateMoralle(moralleMod);

                if (eventDescription_UI.fontSize != 16) { eventDescription_UI.fontSize = 16; }
                int rng = Random.Range(1, 4);
                switch (rng) {
                    case 1:
                        eventDescription_UI.text = "You purchase more RAM for " + activeDepartment.GetRandomEmployeeName() + "'s PC. Its so fast now!";
                        break;
                    case 2:
                        eventDescription_UI.text = "You snipe a 2090 Graphics Card for " + activeDepartment.GetRandomEmployeeName() + ". The font is so crisp now!";
                        break;
                    case 3:
                        eventDescription_UI.text = "There was a great deal online, New mouse pad's for everyone!";
                        break;
                    case 4:
                        eventDescription_UI.text = "You approve double RAM for the server's, things are much faster now!";
                        break;
                    default:
                        eventDescription_UI.text = "You approve an upgrade for " + activeDepartment.GetRandomEmployeeName() + "'s PC.";
                        break;
                }
            }
        }
    }

    int CalculateHirePrice() {
        int businessSize = 0;
        foreach(Department dept in departments) {
            businessSize += dept.GetDepartmentSize();
        }

        return (businessSize * 100); 
    }
    #endregion Abilitys
}
