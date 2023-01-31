import { MyAPI } from "../MyAPI"

export const getModels = async (page: number, perPage: number, search?: string) => {
	let request = `Model?page=${page}&perPage=${perPage}`

	if (search && search != "") {
		request += `&search=${search}`
	}

	const response = await MyAPI().get(request)

	return response.data
}
