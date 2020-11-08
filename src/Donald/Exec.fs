[<AutoOpen>]
module Donald.Exec

open System.Data
open System.Data.Common
open System.Threading.Tasks
open FSharp.Control.Tasks

/// Execute query with no results within transction scope
let tranExec 
    (sql : string) 
    (param : DbParam list) 
    (tran : IDbTransaction) : unit =
    use cmd = newCommand sql param tran    
    cmd.ExecuteNonQuery() |> ignore

/// Try to execute query with no results within transction scope
let tryTranExec 
    (sql : string) 
    (param : DbParam list) 
    (tran : IDbTransaction) : DbResult<unit> =
    try
        tranExec sql param tran
        |> DbResult
    with :? DbException as ex -> DbError ex

/// Execute async query with no results within transction scope
let tranExecAsync 
    (sql : string) 
    (param : DbParam list) 
    (tran : IDbTransaction) : Task =
    task {
        use cmd = newCommand sql param tran :?> DbCommand   
        let! _ = cmd.ExecuteNonQueryAsync()
        return ()
    } :> Task

/// Try to execute async query with no results within transction scope
let tryTranExecAsync 
    (sql : string) 
    (param : DbParam list) 
    (tran : IDbTransaction) : Task<DbResult<unit>> =
    task {
        try
            do! tranExecAsync sql param tran 
            return DbResult ()
        with :? DbException as ex -> return DbError ex
    }

/// Execute query with no results
let exec 
    (sql : string) 
    (param : DbParam list) 
    (conn : IDbConnection) : unit =
    use tran = beginTran conn
    tranExec sql param tran
    commitTran tran

/// Try to execute query with no results
let tryExec 
    (sql : string) 
    (param : DbParam list) 
    (conn : IDbConnection) : DbResult<unit> =
    try
        use tran = beginTran conn
        tranExec sql param tran 
        commitTran tran
        DbResult ()
    with :? DbException as ex -> DbError ex

/// Execute async query with no results
let execAsync 
    (sql : string) 
    (param : DbParam list) 
    (conn : IDbConnection) : Task =
    task {
        use tran = beginTran conn
        use cmd = newCommand sql param tran :?> DbCommand
        let! _ = cmd.ExecuteNonQueryAsync()
        commitTran tran
    } :> Task

/// Try to execute query with no results
let tryExecAsync 
    (sql : string) 
    (param : DbParam list) 
    (conn : IDbConnection) : Task<DbResult<unit>> =
    task {
        try
            use tran = beginTran conn
            do! tranExecAsync sql param tran 
            commitTran tran
            return DbResult ()
        with :? DbException as ex -> return DbError ex
    }

/// Execute sproc with no results within transction scope
let tranExecSproc
    (sproc : string) 
    (param : DbParam list) 
    (tran : IDbTransaction) : unit =
    use cmd = newSproc sproc param tran    
    cmd.ExecuteNonQuery() |> ignore

/// Try to execute sproc with no results within transction scope
let tryTranExecSproc
    (sproc : string) 
    (param : DbParam list) 
    (tran : IDbTransaction) : DbResult<unit> =
    try
        tranExecSproc sproc param tran
        |> DbResult
    with :? DbException as ex -> DbError ex

/// Execute async sproc with no results within transction scope
let tranExecSprocAsync 
    (sproc : string) 
    (param : DbParam list) 
    (tran : IDbTransaction) : Task =
    task {
        use cmd = newSproc sproc param tran :?> DbCommand   
        let! _ = cmd.ExecuteNonQueryAsync()
        return ()
    } :> Task

/// Try to execute async sproc with no results within transction scope
let tryTranExecSprocAsync 
    (sproc : string) 
    (param : DbParam list) 
    (tran : IDbTransaction) : Task<DbResult<unit>> =
    task {
        try
            do! tranExecSprocAsync sproc param tran 
            return DbResult ()
        with :? DbException as ex -> return DbError ex
    }

/// Execute sproc with no results
let execSproc
    (sproc : string) 
    (param : DbParam list) 
    (conn : IDbConnection) : unit =
    use tran = beginTran conn
    tranExecSproc sproc param tran
    commitTran tran

/// Try to execute sproc with no results
let tryExecSproc
    (sproc : string) 
    (param : DbParam list) 
    (conn : IDbConnection) : DbResult<unit> =
    try
        use tran = beginTran conn
        tranExecSproc sproc param tran 
        commitTran tran
        DbResult ()
    with :? DbException as ex -> DbError ex

/// Execute async sproc with no results
let execSprocAsync
    (sproc : string) 
    (param : DbParam list) 
    (conn : IDbConnection) : Task =
    task {
        use tran = beginTran conn
        use cmd = newSproc sproc param tran :?> DbCommand
        let! _ = cmd.ExecuteNonQueryAsync()
        commitTran tran
    } :> Task

/// Try to execute sproc with no results
let tryExecSprocAsync
    (sproc : string) 
    (param : DbParam list) 
    (conn : IDbConnection) : Task<DbResult<unit>> =
    task {
        try
            use tran = beginTran conn
            do! tranExecSprocAsync sproc param tran 
            commitTran tran
            return DbResult ()
        with :? DbException as ex -> return DbError ex
    }
