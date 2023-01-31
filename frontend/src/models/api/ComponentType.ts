import { Pagination } from "./Pagination"

export type ComponentTypeAux = {
	id: number | null
	organizationId: number
	componentType: string
}

export type ComponentType = {
	componentsType: ComponentTypeAux
	numReferences: number
}

export interface ComponentTypePaginated extends Pagination {
	results: Array<ComponentType>
}
