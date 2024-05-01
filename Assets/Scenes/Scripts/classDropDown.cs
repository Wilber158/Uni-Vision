using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ClassDropdownManager : MonoBehaviour
{
    public TMP_Dropdown classNameDropdown;
    public TMP_Dropdown classNumberDropdown;

    // Define a data structure to store class information
    public class ClassInfo
    {
        public string className;
        public List<int> classNumbers;
    }

    // List to store class information
    private List<ClassInfo> classList = new List<ClassInfo>();

    // Populate class dropdowns
    void Start()
    {
        // Hardcoded class list
        // Add your classes and class numbers here
        classList.Add(new ClassInfo
        {
            className = "ACCT",
            classNumbers = new List<int> { 201, 202, 301, 303, 401, 409 }
        });

        classList.Add(new ClassInfo
        {
            className = "ACCG",
            classNumbers = new List<int> { 604, 607,608,610,612}
        });
        classList.Add(new ClassInfo
        {
            className = "BIOL",
            classNumbers = new List<int> {103,104,111, 112,113,114, 131,132,134, 181, 183, 191, 192, 207,208, 210, 217, 218,221,222,223,231,287,291,292,293,301,310, 311, 313,320,321,326,327,390,391,404,406,410,411,413,414}
        });
        classList.Add(new ClassInfo
        {
            className = "BUAN",
            classNumbers = new List<int> { 205,227,310,327,405,410,427}
        });
        classList.Add(new ClassInfo
        {
            className = "CHML",
            classNumbers = new List<int> {201,202,205,207,208,209,211,305,306,316,321,339,342,403,404,405,406,453,458,459,460,470,473,474 }
        });
        classList.Add(new ClassInfo
        {
            className = "CHMG",
            classNumbers = new List<int> {707,713,722,753,758,759,760,770,773,774}
        }); classList.Add(new ClassInfo
        {
            className = "CHEM",
            classNumbers = new List<int> {101,102,103,104,302,309,310,311,319,320,323,324,410,415,421,433,436,437,457,460}
        }); classList.Add(new ClassInfo
        {
            className = "CEEN",
            classNumbers = new List<int> {303,304,307,308,314,405,406,411,418,424,446}
        }); classList.Add(new ClassInfo
        {
            className = "CIVL",
            classNumbers = new List<int> {201,202,302,305,306,309,310,311,312,406,409,410,411,412,414,415,425,428,445,446}
        }); classList.Add(new ClassInfo
        {
            className = "CIVG",
            classNumbers = new List<int> {501,505,508,520,546,777,778,779,784,789,791}
        }); classList.Add(new ClassInfo
        {
            className = "COMM",
            classNumbers = new List<int> {101,103,104,110,120,150,201,209,213,216,217,218,222,225,235,250,270,301,304,305,308,309,315,318,320,330,335,350,375,409,419,420,431,432,433,475}
        }); classList.Add(new ClassInfo
        {
            className = "CIS",
            classNumbers = new List<int> {110,205,310,427}
        }); classList.Add(new ClassInfo
        {
            className = "CMPT",
            classNumbers = new List<int> {101,102,155,238,353,360,364,439,456,466,477,490}
        }); classList.Add(new ClassInfo
        {
            className = "CMPG",
            classNumbers = new List<int> {638,667,756,764,767,798}
        }); classList.Add(new ClassInfo
        {
            className = "COMG",
            classNumbers = new List<int> { 605, 614, 615, 618, 619, 624, 629, 632 }
        });
        classList.Add(new ClassInfo
        {
            className = "DAAS",
            classNumbers = new List<int> { 101, 102, 201, 202, 301, 302, 401, 402, 502 }
        });
        classList.Add(new ClassInfo
        {
            className = "ECON",
            classNumbers = new List<int> { 203, 204, 302, 305, 332, 433 }
        });
        classList.Add(new ClassInfo
        {
            className = "EDUC",
            classNumbers = new List<int> { 201, 206, 303, 311, 353, 357, 376, 377, 378, 380, 401, 408, 418, 438, 444, 446, 453, 454 }
        });
        classList.Add(new ClassInfo
        {
            className = "EDUG",
            classNumbers = new List<int> { 713, 714, 721, 723, 725, 726, 735, 747, 748, 766, 778, 781, 787, 802, 805, 807, 812, 819, 821, 830, 833, 858, 863, 867, 889, 903, 904, 905, 907, 910, 914 }
        });
        classList.Add(new ClassInfo
        {
            className = "ECEG",
            classNumbers = new List<int> {729,738,760,777}
        });
        classList.Add(new ClassInfo
        {
            className = "EECE",
            classNumbers = new List<int> { 201, 210, 229, 303, 305, 307, 321, 410, 439, 443, 462, 465, 471, 476, 477 }
        });
        classList.Add(new ClassInfo
        {
            className = "ENGG",
            classNumbers = new List<int> {614,682}
        });
        classList.Add(new ClassInfo
        {
            className = "ENGS",
            classNumbers = new List<int> {115,204,205,206,230,301}
        });
        classList.Add(new ClassInfo
        {
            className = "ENGL",
            classNumbers = new List<int> { 110, 150, 151, 210, 211, 212, 245, 260, 276, 285, 287, 293, 306, 329, 347, 356, 372 }
        });
        classList.Add(new ClassInfo
        {
            className = "ENVL",
            classNumbers = new List<int> {402,407,409}
        });
        classList.Add(new ClassInfo
        {
            className = "ENVG",
            classNumbers = new List<int> {506,507,508,704,712,718,731}
        });
        classList.Add(new ClassInfo
        {
            className = "FIN",
            classNumbers = new List<int> {301,308,309,324,380,416,432,436}
        });
        classList.Add(new ClassInfo
        {
            className = "ART",
            classNumbers = new List<int> { 134, 145, 150, 151, 212, 213, 214, 307, 309, 380, 409}
        });
        classList.Add(new ClassInfo
        {
            className = "HIST",
            classNumbers = new List<int> {150,152,206,217,231,240,308,334}
        });
        classList.Add(new ClassInfo
        {
            className = "KIN",
            classNumbers = new List<int> { 100, 102, 110, 113, 209, 213, 217, 231, 245, 246, 303, 306, 307, 318, 331, 415, 416, 418, 421, 423, 428, 430, 445 }
        });
        classList.Add(new ClassInfo
        {
            className = "MSOL",
            classNumbers = new List<int> { 601, 605, 610, 615, 620, 642, 683, 690, 691 }
        });
        classList.Add(new ClassInfo
        {
            className = "MGMT",
            classNumbers = new List<int> { 201, 303, 307, 309, 315, 320, 406, 430, 460, 461, 475 }
        });
        classList.Add(new ClassInfo
        {
            className = "MKTG",
            classNumbers = new List<int> {201,303,307,315,403,404,412}
        });
        classList.Add(new ClassInfo
        {
            className = "MBA",
            classNumbers = new List<int> { 611, 617, 618, 622, 630, 635, 636, 637, 639, 640, 652, 710, 720 }
        });
        classList.Add(new ClassInfo
        {
            className = "MATH",
            classNumbers = new List<int> { 100, 111, 151, 153, 154, 155, 185, 186, 187, 221, 230, 243, 285, 286, 322, 331, 351, 372, 377, 386, 471 }
        });
        classList.Add(new ClassInfo
        {
            className = "MECH",
            classNumbers = new List<int> { 211, 312, 318, 321, 323, 401, 405, 411, 414, 472, 474, 477, 478, 486 }
        });
        classList.Add(new ClassInfo
        {
            className = "MECG",
            classNumbers = new List<int> {515,531,541,605,608,630,746}
        });
        classList.Add(new ClassInfo
        {
            className = "MUSC",
            classNumbers = new List<int> { 101, 110, 129, 130, 131, 132, 133, 150, 208, 209, 220, 240, 258, 259, 290, 308, 309, 380, 390, 393, 395 }
        });
        classList.Add(new ClassInfo
        {
            className = "PHIL",
            classNumbers = new List<int> { 150, 152, 201, 208, 210, 211, 214, 308, 334, 350, 375 }
        });
        classList.Add(new ClassInfo
        {
            className = "PHYS",
            classNumbers = new List<int> { 101, 102, 105, 107, 191, 192, 193, 195, 209, 233, 350, 440, 443 }
        });
        classList.Add(new ClassInfo
        {
            className = "POSC",
            classNumbers = new List<int> { 150, 203, 209, 210, 251, 306, 346, 351, 375 }
        });
        classList.Add(new ClassInfo
        {
            className = "PSYC",
            classNumbers = new List<int> { 150, 153, 203, 257, 314, 321, 334, 340, 345, 374, 414, 421, 429, 435 }
        });
        classList.Add(new ClassInfo
        {
            className = "RHS",
            classNumbers = new List<int> { 205, 220, 315, 317, 326, 331, 355, 357, 360, 412, 420, 435, 436, 440, 442, 448, 450, 451, 460, 471 }
        });
        classList.Add(new ClassInfo
        {
            className = "RELS",
            classNumbers = new List<int> { 110, 161, 200, 202, 207, 210, 218, 225, 233, 255, 256, 306, 314, 324, 349, 357, 373, 381, 390 }
        });
        classList.Add(new ClassInfo
        {
            className = "SCI",
            classNumbers = new List<int> { 100, 201, 202, 203, 205, 210, 221, 301 }
        });
        classList.Add(new ClassInfo
        {
            className = "SOC",
            classNumbers = new List<int> { 150, 153, 201, 211, 220, 273, 290, 294, 307, 324 }
        });
        // Populate classNameDropdown with class names
        PopulateClassNameDropdown();

        // Add listener for value changes in classNameDropdown
        classNameDropdown.onValueChanged.AddListener(delegate {
            OnClassNameDropdownValueChanged(classNameDropdown);
        });
    }

    // Populate classNameDropdown with class names
    void PopulateClassNameDropdown()
    {
        // Clear options in classNameDropdown
        classNameDropdown.ClearOptions();

        // Add class names to classNameDropdown
        foreach (ClassInfo info in classList)
        {
            classNameDropdown.options.Add(new TMP_Dropdown.OptionData(info.className));
        }

        // Refresh dropdown options
        classNameDropdown.RefreshShownValue();
    }

    // Update class number dropdown options when classNameDropdown value changes
    void OnClassNameDropdownValueChanged(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;
        string selectedClassName = dropdown.options[index].text;

        // Find selected class info
        ClassInfo selectedClass = classList.Find(x => x.className == selectedClassName);

        if (selectedClass != null)
        {
            // Clear options in classNumberDropdown
            classNumberDropdown.ClearOptions();

            // Add class numbers to classNumberDropdown
            foreach (int number in selectedClass.classNumbers)
            {
                classNumberDropdown.options.Add(new TMP_Dropdown.OptionData(number.ToString()));
            }

            // Refresh dropdown options
            classNumberDropdown.RefreshShownValue();
        }
    }
}
