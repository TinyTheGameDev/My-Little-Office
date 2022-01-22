using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee
{
    string name;
    string hobbie;
    int gender; //1: male, 2: female

    public Employee() {
        //Pick a Sex
        RandomGender();

        //Pick a Name
        RandomName();

        //Pick a Hobbie
        RandomHobbie();
    }

    public Employee(string name) {
        this.name = name;
        RandomHobbie();
    }

    public string GetName() {
        return name;
    }

    public string GetHobbie() {
        return hobbie;
    }

    void RandomGender() {
        gender = Random.Range(1, 2);
    }

    List<string> names = new List<string>() {
        "Sheldon", "Gretchen", "Kaitlynn", "Alisa", "Christian",
        "Marcus", "Yandel", "Juliana", "Amaris", "Arthur",
        "Uriah", "Makenna", "Karma", "Haley", "Kaliyah",
        "Charlie", "Deven", "Phoenix", "Emilee", "Cherish",
        "Halle", "Cyrus", "Hazel", "Aydan", "Adalynn",
        "Jazmyn", "Brooklynn", "Elsie", "Marianna", "Tamia",

        //Hand picked names, cameos, and easter eggs ;)
        "Neo", "Redguard", "Tari", "Lorenzo", "Katie",
        "Lisa", "Nathan", "Michael", "Bruce", "Bill",
        "Bob", "Shalysa", "Alan", "Jim", "Amanda",
        "Katie", "Debbie", "Gibi", "Lara Croft"
    };
    void RandomName() {
        int rng = Random.Range(0, names.Count - 1);
        this.name = names[rng];
    }

    List<string> hobbies = new List<string>() {
        "are a parent of three boys.", "are a parent of two girls.", "dream to be a Video Game Developer.", "enjoy long walks on the beach",
        "can speak 10 languages fluently!", "are studying in college to be a lawyer", "watched the entire Lord Of The Rings Extended series in one night!",
        "have been with the office for 20 years!", "played professional football for two years!", "are a quantum-physics major!"

    };
    void RandomHobbie() {
        switch (name) {
            case "Neo":
                hobbie = " know Kungfu";
                break;
            case "Redguard":
                hobbie = " used to be an adventurer... until they took an arrow to the knee";
                break;
            case "Tari":
                hobbie = " are secretly plotting the office's downfall... shh dont tell anyone!";
                break;
            case "Jim":
                hobbie = " secretly have a crush on Pam the recepionist!";
                break;
            case "Laura Croft":
                hobbie = " discovered an ancient tomb, and raided it!";
                break;
            default:
                int rng = Random.Range(0, hobbies.Count - 1);
                this.hobbie = hobbies[rng];
                break;
        }
    }
}
