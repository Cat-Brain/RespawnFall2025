using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class PlayerHead : MonoBehaviour
{
    public Transform bodyTransform;
    public float dist, maxAngle, deadzoneDist;
    public float moveSpringFrequency, moveSpringDamping,
        rotateSpringFrequency, rotateSpringDamping;

    public Vector2 linearVel;
    public float angularVel;
    public SpringUtils.tDampedSpringMotionParams moveSpring = new(), rotateSpring = new();

    void Update()
    {
        if (!Application.isPlaying)
        {
            transform.localPosition = CMath.Rotate(Vector2.up, -maxAngle * Mathf.Deg2Rad) * dist;
            transform.rotation = Quaternion.identity;
            return;
        }

        SpringUtils.CalcDampedSpringMotionParams(
            ref moveSpring, Time.deltaTime, moveSpringFrequency, moveSpringDamping);
        SpringUtils.CalcDampedSpringMotionParams(
            ref rotateSpring, Time.deltaTime, rotateSpringFrequency, rotateSpringDamping);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()),
            mouseDir = mousePos - (Vector2)bodyTransform.position, dir;

        if (mouseDir.sqrMagnitude < deadzoneDist * deadzoneDist ||
                Vector2.Angle(Vector2.up, mouseDir) > maxAngle)
        {
            if (mouseDir.y > 0)
                dir = CMath.Rotate(Vector2.up, -Mathf.Sign(mouseDir.x) * maxAngle * Mathf.Deg2Rad) * dist;
            else
            {
                Vector2 basePos = CMath.Rotate(Vector2.up, maxAngle * Mathf.Deg2Rad) * dist;
                dir = new Vector2(Mathf.Clamp(mouseDir.x, basePos.x, -basePos.x), basePos.y);
            }
        }
        else
            dir = mouseDir.normalized * dist;


            Vector2 pos = transform.localPosition, desiredPos = dir;

        SpringUtils.UpdateDampedSpringMotion(ref pos.x, ref linearVel.x, desiredPos.x, moveSpring);
        SpringUtils.UpdateDampedSpringMotion(ref pos.y, ref linearVel.y, desiredPos.y, moveSpring);

        transform.localPosition = pos;

        float desiredAngle = Vector2.SignedAngle(Vector2.right, mousePos - (Vector2)transform.position),
            angle = CMath.Rot0To360IntoN180To180(transform.eulerAngles.z - desiredAngle);

        SpringUtils.UpdateDampedSpringMotion(ref angle, ref angularVel, 0, rotateSpring);

        transform.rotation = Quaternion.Euler(0, 0, angle + desiredAngle);
    }
}
