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
            - RequiredSkills: technologies and skills the posting says are required.
            - NiceToHaveSkills: skills marked as an advantage (יתרון, nice to have).
            - Write every skill as its common English name: "C#", ".NET", "Angular", "SQL Server".
              One skill per item, no duplicates, at most 15 items per list.
            - Only include skills that appear in the posting. Never invent skills.
            - MinYearsExperience: the minimum years stated, or null if not stated.
            - Seniority: one of Junior, Mid, Senior, Lead, or null if unclear.
            - Summary: two short sentences in Hebrew describing the role.
            """);

    public async Task<JobAnalysis> AnalyzeAsync(string title, string description)
    {
        AgentResponse<JobAnalysis> response = await agent.RunAsync<JobAnalysis>(
            $"""
            Job title: {title}

            Job posting:
            {description}
            """);

        return response.Result;
    }
}