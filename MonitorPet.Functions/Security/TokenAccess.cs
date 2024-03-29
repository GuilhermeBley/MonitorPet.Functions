﻿namespace MonitorPet.Functions.Security;

internal class TokenAccess
{
    private string _tokenSideByServer { get; }

    public TokenAccess(string tokenSideByServer)
    {
        _tokenSideByServer = tokenSideByServer;
    }

    public bool IsValidAccessToken(string tokenToCheck)
    {
        var tokenSideByServer = _tokenSideByServer;

        if (string.IsNullOrEmpty(tokenSideByServer) ||
            string.IsNullOrEmpty(tokenToCheck) ||
            tokenSideByServer != tokenToCheck)
            return false;

        return true;
    }
}

