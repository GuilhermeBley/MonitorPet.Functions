using MonitorPet.Functions.Model;
using MonitorPet.Functions.MySqlConnection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapper;

namespace MonitorPet.Functions.Repository;

internal interface IUserRepository
{
    /// <summary>
    /// Users with configured e-mails by id dosador
    /// </summary>
    Task<IEnumerable<QueryUserJoinEmailModel>> GetByDosador(Guid idDosador);
}

internal class UserRepository : IUserRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public UserRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<QueryUserJoinEmailModel>> GetByDosador(Guid idDosador)
    {
        const string QUERY = @"
SELECT
    u.Email Email,
    u.Nome UserName,
    et.TipoEnvio EmailType
FROM monitorpet.usuario u
INNER JOIN monitorpet.dosadorusuario d
	ON u.IdUsuario = d.IdUsuario
INNER JOIN monitorpet.regraemailuser e
	ON e.IdUsuario = d.IdUsuario
INNER JOIN monitorpet.tipoemail et
	ON et.Id = e.IdTipoEmail
WHERE d.IdDosador = @IdDosador;
";

        var connection = await _connectionFactory.CreateOpenConnection();

        return await connection.QueryAsync<QueryUserJoinEmailModel>(
            QUERY,
            new { IdDosador = idDosador }
        );
    }
}
