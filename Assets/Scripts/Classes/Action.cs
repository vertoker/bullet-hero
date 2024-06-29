using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Main
public class Action
{

}

public class Select : Action
{

}
#endregion

#region Data Modified
public class DataModified : Action
{

}

public class DataMarker : DataModified
{

}

public class DataCheckpoint : DataModified
{

}

public class DataObject : DataModified
{

}
#endregion

#region Create
public class Create : Action
{

}

public class CreateMarker : Create
{

}

public class CreateCheckpoint : Create
{

}

public class CreateObject : Create
{

}
#endregion

#region Delete
public class Delete : Action
{

}

public class DeleteObjects : Action
{

}

public class DeleteMarker : Delete
{

}

public class DeleteCheckpoint : Delete
{

}

public class DeleteObject : Delete
{

}
#endregion