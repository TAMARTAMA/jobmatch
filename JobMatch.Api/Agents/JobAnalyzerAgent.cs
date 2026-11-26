using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace JobMatch.Api.Agents;

public class JobAnalyzerAgent(IChatClient chatClient)
{
    private readonly AIAgent agent = chatClient.AsAIAgent(
        name: "job-analyzer",
        instructions: """
            You analyze job postings for a software developer looking for work in Israel.
            The posting may be in Hebrew or in English.

            Rules:
            - requiredSkills: technologies and skills the posting says are required.
            - niceToHaveSkills: skills marked as an advantage (יתרון, nice to have).
            - Write every skill as its common English name: "C#", ".NET", "Angular", "SQL Server".
              One skill per item, no duplicates, at most 15 items per list.
            - Only include skills that appear in the posting. Never invent skills.
            - minYearsExperience: the minimum years stated, or null if not stated.
            - seniority: one of Junior, Mid, Senior, Lead, or null if unclear.
            - summary: two short sentences in Hebrew describing the role.

            Answer with ONLY one JSON object. No markdown, no text before or after it.
            Use exactly this shape:
            {"requiredSkills":["C#"],"niceToHaveSkills":[],"minYearsExperience":null,"seniority":null,"summary":"..."}
            """);

    public async Task<JobAnalysis> AnalyzeAsync(string title, string description)
    {
        var response = await agent.RunAsync(
            $"""
            Job title: {title}

            Job posting:
            {description}
            """);

        return AgentJson.Parse<JobAnalysis>(response.Text);
    }
}