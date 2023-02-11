export interface ICategory {
    id: string;
    name: string;
    parentId: string;
    children?: this[];
}

export interface ICategoryTree extends ICategory{    
    level: number;
}
