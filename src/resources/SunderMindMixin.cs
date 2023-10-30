using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using XRL;
using XRL.World;
using XRL.World.Parts.Mutation;
using XRL.UI;

namespace at.petrak.issrdol {
public class SunderMind_Patch {
  public static MethodInfo POPUP_SHOW = AccessTools.Method(
    typeof(Popup),
    "Show",
    // Objective: futz with the string
    // The bools are all true
    new [] { typeof(string), typeof(bool), typeof(bool), typeof(bool), typeof(bool) }
  );

  [HarmonyPatch(typeof(SunderMind))]
  [HarmonyPatch(nameof(SunderMind.BeginSunder))]
  static class BeginSunder_Patch {
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> code) {
      bool found = false;
      foreach (var op in code) {
        if (op.Calls(POPUP_SHOW)) {
          found = true;
          // load the this
          yield return new CodeInstruction(OpCodes.Ldarg_0);
          yield return CodeInstruction.Call(typeof(SunderMind_Patch), nameof(patch_StartBurrow));
          // Put the four bools on the stack
          for (int i = 0; i < 4; i++) {
            yield return new CodeInstruction(OpCodes.Ldc_I4_1);
          }
        }
        yield return op;      
      }

      if (!found) {
        throw new System.Exception("Never found Popup.Show");
      }
    }
  }

  [HarmonyPatch(typeof(SunderMind))]
  [HarmonyPatch(nameof(SunderMind.Tick))]
  static class FinishSunder_Patch {
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> code) {
      foreach (var op in code) {
        if (op.LoadsConstant("Your sense of self is pulled apart by what feels like a billion years of geologic pressure.")) {
          yield return new CodeInstruction(OpCodes.Ldstr, "\"Who's Resheph?\"");
        } else if (op.LoadsConstant("Your head explodes!")) {
          yield return new CodeInstruction(OpCodes.Ldstr, "{{Y|\"ligma balls\"}}");
        } else {
          yield return op;
        }
      }
    }
  }

  static string patch_StartBurrow(string _msg, bool _1, bool _2, bool _3, bool _4,
    SunderMind s) {
    GameObject sunderer = s.ParentObject;
    return sunderer.T() + " " + The.Player.DescribeDirectionToward(sunderer) +
      sunderer.GetVerb("burrow") + " a channel through the psychic aether and " +
      sunderer.GetVerb("begin") + " to say {{Y|\"It's so sad that Resheph died of ligma.\"}}";
  }
} }
