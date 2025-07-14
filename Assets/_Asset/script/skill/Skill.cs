using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    // Tên kỹ năng để hiển thị/debug
    public string skillName;

    // Hàm thi triển kỹ năng - các class con phải tự định nghĩa
    public abstract void Use();

}
