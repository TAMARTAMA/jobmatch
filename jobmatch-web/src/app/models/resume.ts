
export interface Resume {
    id: number;
    title: string;
    content: string;
    createdAt: string;
}

export interface CreateResume {
    title: string;
    content: string;
}
