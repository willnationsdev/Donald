[<AutoOpen>]
module Donald.Query

open System.Data
open System.Data.Common
open FSharp.Control.Tasks

/// Query for multiple results within transaction scope
let tranQuery (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    use cmd = newCommand sql param tran
    use rd = cmd.ExecuteReader()
    let results = [ while rd.Read() do yield map rd ]
    rd.Close() |> ignore
    results

/// Try to query for multiple results within transaction scope
let tryTranQuery (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    try
        tranQuery sql param map tran
        |> DbResult 
    with :? DbException as ex -> DbError ex

/// Query async for multiple results within transaction scope
let tranQueryAsync (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    task {
        use cmd = newCommand sql param tran :?> DbCommand
        use! rd = cmd.ExecuteReaderAsync()        

        let rec loopAsync (acc : 'a list) (rd : DbDataReader) =
            task {
                let! canRead = rd.ReadAsync()
                match canRead with
                | false -> return acc
                | true  -> 
                    let result = map rd
                    let results = result :: acc
                    return! loopAsync results rd
            }

        let! results = loopAsync [] rd
        rd.Close() |> ignore
        return results
    }

/// Try to query for multiple results within transaction scope
let tryTranQueryAsync (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    task {
        try
            let! result = tranQueryAsync sql param map tran
            return DbResult result
        with :? DbException as ex -> return DbError ex
    }

/// Query for multiple results
let query (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =
    use tran = beginTran conn
    let results = tranQuery sql param map tran
    commitTran tran
    results

/// Query for multiple results
let tryQuery (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =   
    try
        use tran = beginTran conn
        let results = tranQuery sql param map tran
        commitTran tran
        DbResult results
    with :? DbException as ex -> DbError ex
    
/// Query async for multiple results
let queryAsync (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =
    task {
        use tran = beginTran conn
        let! results = tranQueryAsync sql param map tran
        commitTran tran
        return results
    }

/// Query async for multiple results
let tryQueryAsync (sql : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =   
    task {
        try
            use tran = beginTran conn
            let! results = tranQueryAsync sql param map tran
            commitTran tran
            return DbResult results
        with :? DbException as ex -> return DbError ex
    }

/// Sproc for multiple results within transaction scope
let tranQuerySproc (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    use cmd = newSproc sproc param tran
    use rd = cmd.ExecuteReader()
    let results = [ while rd.Read() do yield map rd ]
    rd.Close() |> ignore
    results

/// Try to query for multiple results within transaction scope
let tryTranQuerySproc (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    try
        tranQuerySproc sproc param map tran
        |> DbResult 
    with :? DbException as ex -> DbError ex

/// Sproc async for multiple results within transaction scope
let tranQuerySprocAsync (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    task {
        use cmd = newSproc sproc param tran :?> DbCommand
        use! rd = cmd.ExecuteReaderAsync()        

        let rec loopAsync (acc : 'a list) (rd : DbDataReader) =
            task {
                let! canRead = rd.ReadAsync()
                match canRead with
                | false -> return acc
                | true  -> 
                    let result = map rd
                    let results = result :: acc
                    return! loopAsync results rd
            }

        let! results = loopAsync [] rd
        rd.Close() |> ignore
        return results
    }

/// Try to query for multiple results within transaction scope
let tryTranQuerySprocAsync (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (tran : IDbTransaction) =
    task {
        try
            let! result = tranQuerySprocAsync sproc param map tran
            return DbResult result
        with :? DbException as ex -> return DbError ex
    }

/// Sproc for multiple results
let querySproc (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =
    use tran = beginTran conn
    let results = tranQuerySproc sproc param map tran
    commitTran tran
    results

/// Sproc for multiple results
let tryQuerySproc (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =   
    try
        use tran = beginTran conn
        let results = tranQuerySproc sproc param map tran
        commitTran tran
        DbResult results
    with :? DbException as ex -> DbError ex
    
/// Sproc async for multiple results
let querySprocAsync (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =
    task {
        use tran = beginTran conn
        let! results = tranQuerySprocAsync sproc param map tran
        commitTran tran
        return results
    }

/// Sproc async for multiple results
let tryQuerySprocAsync (sproc : string) (param : DbParam list) (map : IDataReader -> 'a) (conn : IDbConnection) =   
    task {
        try
            use tran = beginTran conn
            let! results = tranQuerySprocAsync sproc param map tran
            commitTran tran
            return DbResult results
        with :? DbException as ex -> return DbError ex
    }
