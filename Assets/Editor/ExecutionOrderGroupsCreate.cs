using UnityEditor;
 
public class ExecutionOrderGroupsCreate
{
    [MenuItem( "Assets/Create/ExecutionOrderGroups" )]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<ExecutionOrderGroups>();
	}
}
