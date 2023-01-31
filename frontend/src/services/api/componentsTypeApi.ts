import { ComponentTypeAux } from "../../models/api/ComponentType"
import { MyAPI } from "../MyAPI"

export const getComponentTypes = async (
	page: number = 1,
	perPage: number = 10,
	search: string = ""
) => {
	const response = await MyAPI().get(
		`ComponentType?page=${page}&perPage=${perPage}&search=${search}`
	)
	return response.data
}

export const addComponentType = async (props: ComponentTypeAux) => {
	const newComponentType: any = props
	newComponentType.id = undefined
	return await MyAPI().post("/ComponentType", newComponentType)
}

export const deleteComponentType = async (props: { id: number }) => {
	return await MyAPI().delete(`/ComponentType?id=${props.id}`)
}
