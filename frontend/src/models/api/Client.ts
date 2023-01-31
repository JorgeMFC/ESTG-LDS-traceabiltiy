import { Pagination } from "./Pagination"

export interface Client {
	id: string
	organizationId: number
	name: string
	email: string
}

export interface Address {
	id: string
	clientId: string
	organizationId: number
	address: string
	zipcode: string
}

export interface ClientAddressList {
	client: Client
	clientsAddressesList: Array<Address>
}

export interface ClientPaginated extends Pagination {
	results: Array<Client>
}
