[<AutoOpen>]
module Donald.Scalar

open System.Data
open System.Data.Common
open FSharp.Control.Tasks

/// Execute query that returns scalar result within transcation scope
let tranScalar (sql : string) (param : DbParam list) (convert : obj -> 'a) (tran : IDbTransaction) =
    use cmd = newCommand sql param tran
    convert (cmd.ExecuteScalar())

/// Try to execute query that returns scalar result within transcation scope
let tryTranScalar (sql : string) (param : DbParam list) (convert : obj -> 'a) (tran : IDbTransaction) =
    try
        tranScalar sql param convert tran
        |> DbResult
    with :? DbException as ex -> DbError ex

/// Execute query async that returns scalar result within transcation scope
let tranScalarAsync (sql : string) (param : DbParam list) (convert : obj -> 'a) (tran : IDbTransaction) =
    task {
        use cmd = newCommand sql param tran :?> DbCommand
        let! result = cmd.ExecuteScalarAsync()
        return convert (result)
    }

/// Try to execute query async that returns scalar result within transcation scope
let tryTranScalarAsync (sql : string) (param : DbParam list) (convert : obj -> 'a) (tran : IDbTransaction) =
    task {
        try
            let! result = tranScalarAsync sql param convert tran
            return DbResult result
        with :? DbException as ex -> return DbError ex
    }

/// Execute query with scalar result
let scalar (sql : string) (param : DbParam list) (convert : obj -> 'a) (conn : IDbConnection) =
    use tran = beginTran conn
    let v = tranScalar sql param convert tran
    commitTran tran
    v

/// Try to execute query async with scalar result
let tryScalarAsync (sql : string) (param : DbParam list) (convert : obj -> 'a) (conn : IDbConnection) =
    task {
        try
            use tran = beginTran conn
            let! result = tranScalarAsync sql param convert tran
            commitTran tran
            return DbResult result
        with :? DbException as ex -> return DbError ex
    }   

/// Execute query async with scalar result
let scalarAsync (sql : string) (param : DbParam list) (convert : obj -> 'a) (conn : IDbConnection) =
    task {
        use tran = beginTran conn
        let! v = tranScalarAsync sql param convert tran
        commitTran tran
        return v
    }

/// Try to execute query with scalar result
let tryScalar (sql : string) (param : DbParam list) (convert : obj -> 'a) (conn : IDbConnection) =
    try
        use tran = beginTran conn
        let result = tranScalar sql param convert tran
        commitTran tran
        DbResult result
    with :? DbException as ex -> DbError ex    

/// Execute sproc that returns scalar result within transcation scope
let tranScalarSproc (sproc : string) (param : DbParam list) (convert : obj -> 'a) (tran : IDbTransaction) =
    use cmd = newSproc sproc param tran
    convert (cmd.ExecuteScalar())

/// Try to execute sproc that returns scalar result within transcation scope
let tryTranScalarSproc (sproc : string) (param : DbParam list) (convert : obj -> 'a) (tran : IDbTransaction) =
    try
        tranScalarSproc sproc param convert tran
        |> DbResult
    with :? DbException as ex -> DbError ex

/// Execute query async that returns scalar result within transcation scope
let tranScalarSprocAsync (sproc : string) (param : DbParam list) (convert : obj -> 'a) (tran : IDbTransaction) =
    task {
        use cmd = newSproc sproc param tran :?> DbCommand
        let! result = cmd.ExecuteScalarAsync()
        return convert (result)
    }

/// Try to execute query async that returns scalar result within transcation scope
let tryTranScalarSprocAsync (sproc : string) (param : DbParam list) (convert : obj -> 'a) (tran : IDbTransaction) =
    task {
        try
            let! result = tranScalarSprocAsync sproc param convert tran
            return DbResult result
        with :? DbException as ex -> return DbError ex
    }

/// Execute query with scalar result
let scalarSproc (sproc : string) (param : DbParam list) (convert : obj -> 'a) (conn : IDbConnection) =
    use tran = beginTran conn
    let v = tranScalarSproc sproc param convert tran
    commitTran tran
    v

/// Try to execute query async with scalar result
let tryScalarSprocAsync (sproc : string) (param : DbParam list) (convert : obj -> 'a) (conn : IDbConnection) =
    task {
        try
            use tran = beginTran conn
            let! result = tranScalarSprocAsync sproc param convert tran
            commitTran tran
            return DbResult result
        with :? DbException as ex -> return DbError ex
    }   

/// Execute query async with scalar result
let scalarSprocAsync (sproc : string) (param : DbParam list) (convert : obj -> 'a) (conn : IDbConnection) =
    task {
        use tran = beginTran conn
        let! v = tranScalarSprocAsync sproc param convert tran
        commitTran tran
        return v
    }

/// Try to execute query with scalar result
let tryScalarSproc (sproc : string) (param : DbParam list) (convert : obj -> 'a) (conn : IDbConnection) =
    try
        use tran = beginTran conn
        let result = tranScalarSproc sproc param convert tran
        commitTran tran
        DbResult result
    with :? DbException as ex -> DbError ex    
