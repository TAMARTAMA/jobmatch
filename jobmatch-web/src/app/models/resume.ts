
export interface Resume {
  id: number;
  title: string;
  content: string;
  createdAt: string;
}

export interface SaveResume {
  title: string;
  content: string;
}