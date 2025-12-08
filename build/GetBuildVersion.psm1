function GetBuildVersion {
    param([string]$RefName)

    if ($RefName -match 'refs/tags/v(\d+\.\d+\.\d+)') {
        return $Matches[1]
    }

    # fallback en caso de branch
    return "0.0.0-dev"
}
