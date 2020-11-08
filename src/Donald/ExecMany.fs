[<AutoOpen>]
module Donald.ExecMany

open System.Data
open System.Data.Common
open FSharp.Control.Tasks

/// Execute query with no results many times within transction scope
let tranExecMany (sql : string) (manyParam : DbParam list list) (tran : IDbTransaction) =    
    use cmd = newTextDbCommand sql tran    
    for param in manyParam do
        clearParameters cmd
        assignDbParams cmd param
        cmd.ExecuteNonQuery() |> ignore

/// Try to execute query with no results many times within transction scope
let tryTranExecMany (sql : string) (manyParam : DbParam list list) (tran : IDbTransaction) =    
    try
        tranExecMany sql manyParam tran
        |> DbResult
    with :? DbException as ex -> DbError ex

/// Execute query async with no results many times within transction scope
let tranExecManyAsync (sql : string) (manyParam : DbParam list list) (tran : IDbTransaction) =    
    task {
        use cmd = newTextDbCommand sql tran :?> DbCommand
        for param in manyParam do
            clearParameters cmd
            assignDbParams cmd param
            cmd.ExecuteNonQueryAsync() |> ignore
    }

/// Try to execute a query async with no results many times
let tryExecManyAsync (sql : string) (manyParam : DbParam list list) (conn : IDbConnection) =
    task {
        try
            use tran = beginTran conn
            do! tranExecManyAsync sql manyParam tran
            commitTran tran
            return DbResult ()
        with :? DbException as ex -> return DbError ex
    }
        
/// Execute a query with no results many times
let execMany (sql : string) (manyParam : DbParam list list) (conn : IDbConnection) =
    use tran = beginTran conn
    tranExecMany sql manyParam tran
    commitTran tran

/// Try to execute a query with no results many times
let tryExecMany (sql : string) (manyParam : DbParam list list) (conn : IDbConnection) =
    try
        use tran = beginTran conn
        tranExecMany sql manyParam tran
        commitTran tran
        DbResult ()
    with :? DbException as ex -> DbError ex

/// Execute a query async with no results many times
let execManyAsync (sql : string) (manyParam : DbParam list list) (conn : IDbConnection) =
    task {
        use tran = beginTran conn
        do! tranExecManyAsync sql manyParam tran
        commitTran tran
    }

/// Try to execute query async with no results many times within transction scope
let tryTranExecManyAsync (sql : string) (manyParam : DbParam list list) (tran : IDbTransaction) =    
    task { 
        try
            do! tranExecManyAsync sql manyParam tran
            return DbResult ()
        with :? DbException as ex -> return DbError ex
    }

/// Execute sproc with no results many times within transction scope
let tranExecManySproc (sproc : string) (manyParam : DbParam list list) (tran : IDbTransaction) =    
    use cmd = newSprocDbCommand sproc tran    
    for param in manyParam do
        clearParameters cmd
        assignDbParams cmd param
        cmd.ExecuteNonQuery() |> ignore

/// Try to execute sproc with no results many times within transction scope
let tryTranExecManySproc (sproc : string) (manyParam : DbParam list list) (tran : IDbTransaction) =    
    try
        tranExecManySproc sproc manyParam tran
        |> DbResult
    with :? DbException as ex -> DbError ex

/// Execute sproc async with no results many times within transction scope
let tranExecManySprocAsync (sproc : string) (manyParam : DbParam list list) (tran : IDbTransaction) =    
    task {
        use cmd = newSprocDbCommand sproc tran :?> DbCommand
        for param in manyParam do
            clearParameters cmd
            assignDbParams cmd param
            cmd.ExecuteNonQueryAsync() |> ignore
    }

/// Try to execute a sproc async with no results many times
let tryExecManySprocAsync (sproc : string) (manyParam : DbParam list list) (conn : IDbConnection) =
    task {
        try
            use tran = beginTran conn
            do! tranExecManySprocAsync sproc manyParam tran
            commitTran tran
            return DbResult ()
        with :? DbException as ex -> return DbError ex
    }
        
/// Execute a sproc with no results many times
let execManySproc (sproc : string) (manyParam : DbParam list list) (conn : IDbConnection) =
    use tran = beginTran conn
    tranExecManySproc sproc manyParam tran
    commitTran tran

/// Try to execute a sproc with no results many times
let tryExecManySproc (sproc : string) (manyParam : DbParam list list) (conn : IDbConnection) =
    try
        use tran = beginTran conn
        tranExecManySproc sproc manyParam tran
        commitTran tran
        DbResult ()
    with :? DbException as ex -> DbError ex

/// Execute a sproc async with no results many times
let execManySprocAsync (sproc : string) (manyParam : DbParam list list) (conn : IDbConnection) =
    task {
        use tran = beginTran conn
        do! tranExecManySprocAsync sproc manyParam tran
        commitTran tran
    }

/// Try to execute sproc async with no results many times within transction scope
let tryTranExecManySprocAsync (sproc : string) (manyParam : DbParam list list) (tran : IDbTransaction) =    
    task { 
        try
            do! tranExecManySprocAsync sproc manyParam tran
            return DbResult ()
        with :? DbException as ex -> return DbError ex
    }

