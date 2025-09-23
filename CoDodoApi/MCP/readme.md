The following is an example `mcp.json` file that sets up the Cododo MCP server.
It should be put inide the `.vscode` directory of this project.

```json
{
	"servers": {
		"cododo-mcp": {
			"type": "stdio",
			"command": "dotnet",
			"args": [
				"run",
				"--project",
				"D:/Personal/CoDodoPublic/CoDodoApi/CoDodoApi.csproj"
			]
		}
	},
	"inputs": []
}
```

An example prompt to try it out is:
"Use the Cododo MCP to figure out the name of the oldest process."