[<AutoOpen>]
module Donald.QuerySingle

open System.Data
open System.Data.Common
open FSharp.Control.Tasks

/// Query for single result within transaction scope
let tranQuerySingle (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    use cmd = newCommand sql param tran
    use rd = cmd.ExecuteReader()
    if rd.Read() then Some(map rd) else None

/// Query async for single result within transaction scope
let tranQuerySingleAsync (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    task {
        use cmd = newCommand sql param tran :?> DbCommand
        use! rd = cmd.ExecuteReaderAsync()
        let! canRead = rd.ReadAsync()
        return if canRead then Some(map rd) else None
    }

/// Query for single result
let querySingle (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =
    use tran = beginTran conn
    let result = tranQuerySingle sql param map tran
    commitTran tran
    result

/// Query for single result
let querySingleAsync (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =
    task {
        use tran = beginTran conn
        let! result = tranQuerySingleAsync sql param map tran
        commitTran tran
        return result
    }

/// Try to query for single result within transaction scope
let tryTranQuerySingle (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    try
        tranQuerySingle sql param map tran
        |> DbResult
    with :? DbException as ex -> DbError ex

/// Try to query async for single result within transaction scope
let tryTranQuerySingleAsync (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    task {
        try
            let! result = tranQuerySingleAsync sql param map tran
            return DbResult result
        with :? DbException as ex -> return DbError ex
    }

/// Query for single result
let tryQuerySingle (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =
    try
        use tran = beginTran conn
        let result = tranQuerySingle sql param map tran 
        commitTran tran
        DbResult result
    with :? DbException as ex -> DbError ex

/// Query for single result
let tryQuerySingleAsync (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =
    task {
        try
            use tran = beginTran conn
            let! result = tranQuerySingleAsync sql param map tran 
            commitTran tran
            return DbResult result
        with :? DbException as ex -> return DbError ex
    }


/// Sproc for single result within transaction scope
let tranQuerySingleSproc (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    use cmd = newSproc sproc param tran
    use rd = cmd.ExecuteReader()
    if rd.Read() then Some(map rd) else None

/// Sproc async for single result within transaction scope
let tranQuerySingleSprocAsync (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    task {
        use cmd = newSproc sproc param tran :?> DbCommand
        use! rd = cmd.ExecuteReaderAsync()
        let! canRead = rd.ReadAsync()
        return if canRead then Some(map rd) else None
    }

/// Sproc for single result
let querySingleSproc (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =
    use tran = beginTran conn
    let result = tranQuerySingleSproc sproc param map tran
    commitTran tran
    result

/// Sproc async for single result
let querySingleSprocAsync (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =
    task {
        use tran = beginTran conn
        let! result = tranQuerySingleSprocAsync sproc param map tran
        commitTran tran
        return result
    }


/// Try to sproc for single result within transaction scope
let tryTranQuerySingleSproc (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    try
        tranQuerySingleSproc sproc param map tran
        |> DbResult
    with :? DbException as ex -> DbError ex

/// Try to sproc async for single result within transaction scope
let tryTranQuerySingleSprocAsync (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    task {
        try
            let! result = tranQuerySingleSprocAsync sproc param map tran
            return DbResult result
        with :? DbException as ex -> return DbError ex
    }

/// Try to sproc for single result
let tryQuerySingleSproc (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =
    try
        use tran = beginTran conn
        let result = tranQuerySingleSproc sproc param map tran 
        commitTran tran
        DbResult result
    with :? DbException as ex -> DbError ex

/// Try to sproc async for single result
let tryQuerySingleSprocAsync (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =
    task {
        try
            use tran = beginTran conn
            let! result = tranQuerySingleSprocAsync sproc param map tran 
            commitTran tran
            return DbResult result
        with :? DbException as ex -> return DbError ex
    }

