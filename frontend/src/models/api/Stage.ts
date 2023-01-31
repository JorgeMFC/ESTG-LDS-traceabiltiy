import { Pagination } from "./Pagination"

export interface Stage {
	id: string
	organizationId: number
	stageName: string
	stagesTypeId: string
	stageDescription: string | null
}

export interface StagePaginated extends Pagination {
	results: Array<Stage>
}
