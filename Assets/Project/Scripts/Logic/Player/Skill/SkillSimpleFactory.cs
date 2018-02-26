using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillSimpleFactory {

    public static BaseSkillCtrl GetSkill(BasePlayerCtrl playerCtrl, string skillName) {
        BaseSkillCtrl skill;
        switch (skillName) {
            case "Burst":
                skill = playerCtrl.gameObject.AddComponent<BurstSkillCtrl>();
                skill.playerCtrl = playerCtrl;
                break;
            case "Jump":
                skill = playerCtrl.gameObject.AddComponent<JumpSkillCtrl>();
                skill.playerCtrl = playerCtrl;
                break;
            case "ReChange":
                skill = playerCtrl.gameObject.AddComponent<ReChangeSkillCtrl>();
                skill.playerCtrl = playerCtrl;
                break;
            case "Shield":
                skill = playerCtrl.gameObject.AddComponent<ShieldSkillCtrl>();
                skill.playerCtrl = playerCtrl;
                break;
            default:
                skill = playerCtrl.gameObject.AddComponent<BaseSkillCtrl>();
                break;
        }

        return skill;
    }
}
