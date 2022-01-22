using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Department
{
    string departmentName;
    int income;
    double productivity;
    double moralle;
    Vector3 departmentPosition;

    List<GameObject> departmentEmployees;

    //Constructor
    public Department(string departmentName, int income, double productivity, double moralle, Vector3 departmentPosition) {
        this.departmentName = departmentName;
        this.income = income;
        this.productivity = productivity;
        this.moralle = moralle;
        this.departmentPosition = departmentPosition;
        this.departmentEmployees = new List<GameObject>();
    }

    //Updates
    public void UpdateIncome(int amount) {
        income += amount;
        //if(income <= departmentEmployees.Count) {
        //    income = departmentEmployees.Count;
        //}
        if(income <= 0) {
            income = 0;
        }
    }

    public void UpdateProductivity(double amount) {
        productivity += amount;
        if (productivity <= 0) {
            productivity = 0;
        }
    }

    public void UpdateMoralle(double amount) {
        moralle += amount;
        if (moralle <= 0) {
            moralle = 0;
        }
    }

    public void AddEmployee(GameObject employee) {
        departmentEmployees.Add(employee);
    }
    //TODO: Maybe allow firing or removing employees. Employees who quit for low moralle?

    //Getters and Setters
    public string GetDepartmentName() {
        return departmentName;
    }

    public int GetDepartmentSize() {
        return departmentEmployees.Count;
    }
    public bool IsDepartmentFull() {
        if(departmentEmployees.Count == 12) {
            return true;
        } else {
            return false;
        }
    }

    public int GetIncome() {
        //Return income unless productivity or moralle are zero
        if (moralle <= 0 || productivity <= 0) {
            return 0;
        } else {
            return Mathf.CeilToInt(income * (float)productivity);
        }
    }

    public double GetProductivity() {
        return productivity;
    }

    public string GetProductivityString() {
        return (productivity * 100).ToString("F0") + "%";
    }

    public double GetMoralle() {
        return moralle;
    }

    public string GetMoralleString() {
        return (moralle * 100).ToString("F0") + "%";
    }

    public Vector3 GetDepartmentPosition() {
        return departmentPosition;
    }

    public GameObject GetEmployee(int index) {
        return departmentEmployees[index];
    }

    public GameObject GetRandomEmployee() {
        int pick = Random.Range(0, departmentEmployees.Count - 1);
        return departmentEmployees[pick];
    }

    public List<GameObject> GetEmployees() {
        return departmentEmployees;
    }

    public string GetRandomEmployeeName() {
        int pick = Random.Range(0, departmentEmployees.Count - 1);
        return departmentEmployees[pick].GetComponent<EmployeeManager>().GetEmployeeName();
    }

}
