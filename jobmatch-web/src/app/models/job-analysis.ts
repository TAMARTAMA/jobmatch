export interface JobSkill {
    name: string;
    isRequired: boolean;
}

export interface JobAnalysis {
    requiredSkills: string[];
    niceToHaveSkills: string[];
    minYearsExperience: number | null;
    seniority: string | null;
    summary: string;
}