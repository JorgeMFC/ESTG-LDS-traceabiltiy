import { Dropdown } from "flowbite-react"
import { useState } from "react"

import DialogRemove from "../../../../components/DialogRemove"

import { FiMoreVertical as MoreIcon } from "react-icons/fi"

import toast from "react-hot-toast"
import { Puff } from "react-loader-spinner"
import { useMutation, useQuery } from "react-query"
import Table from "../../../../components/Table"
import { TableSizes } from "../../../../models/TableSizes"
import { Client, ClientPaginated } from "../../../../models/api/Client"
import { getClients, removeClient } from "../../../../services/api/clientApi"
import { queryClient } from "../../../../services/queryClient"
import ClientsAdd from "../ClientsAdd"
import ClientsEdit from "../ClientsEdit"

export default function ClientsList() {
	const [search, setSearch] = useState("")
	const [page, setPage] = useState(1)
	const [perPage, setPerPage] = useState(10)

	const [openAdd, setOpenAdd] = useState(false)
	const handleVisibilityAdd = (visibility: boolean) => {
		setOpenAdd(visibility)
	}

	const [openEdit, setOpenEdit] = useState(false)
	const handleVisibilityEdit = (visibility: boolean) => {
		setOpenEdit(visibility)
	}

	const [openRemove, setOpenRemove] = useState(false)
	const handleVisibilityRemove = (visibility: boolean) => {
		setOpenRemove(visibility)
	}

	const [currClient, setCurrClient] = useState<Client>({
		email: "",
		id: "",
		name: "",
		organizationId: 1,
	})

	const {
		isLoading: isLoadingClients,
		isError: isErrorClients,
		error: errorClients,
		data: clientsResult,
	} = useQuery<ClientPaginated, Error>(
		["clients-page-clients", page, perPage, search],
		() => getClients(page, perPage, search),
		{ keepPreviousData: true }
	)

	let maxPage = clientsResult?.totalPages || 0

	const previousPage = () => {
		let aux = page
		if (aux > 1) {
			setPage(aux - 1)
		}
	}

	const nextPage = () => {
		let aux = page
		if (aux < maxPage) {
			setPage(aux + 1)
		}
	}

	const removeClientMutation = useMutation(removeClient, {
		onSuccess: (response) => {
			// Invalidates cache and refetch
			queryClient.invalidateQueries("clients-page-clients")
			toast.success("Client removed successfully!")
		},
		onError: (error: Error) => {
			toast.error(error.message)
		},
	})

	const title = ["Name", "Email", ""]
	let content: any[][] = []

	if (isLoadingClients) {
		return (
			<div className="w-full h-full flex justify-center items-center">
				<Puff height="150" width="150" radius={1} color="#3b82f6" visible={true} />
			</div>
		)
	} else if (isErrorClients) {
		return (
			<div>
				<h1 className="font-semibold text-red-600">{errorClients?.message}</h1>
			</div>
		)
	} else {
		clientsResult?.results?.forEach((element) => {
			content.push([
				element.name,
				element.email,
				<div key={element.id}>
					<Dropdown arrowIcon={false} inline={true} label={<MoreIcon />}>
						<Dropdown.Item
							onClick={() => {
								setCurrClient(element)
								handleVisibilityEdit(true)
							}}
						>
							Edit
						</Dropdown.Item>
						<Dropdown.Divider />
						<Dropdown.Item
							onClick={() => {
								setCurrClient(element)
								handleVisibilityRemove(true)
							}}
						>
							Remove
						</Dropdown.Item>
					</Dropdown>
				</div>,
				,
			])
		})
	}

	return (
		<div className="flex justify-center">
			<Table
				size={TableSizes.SMALL}
				titles={title}
				content={content}
				page={page}
				maxPage={maxPage}
				nextPage={nextPage}
				previousPage={previousPage}
				buttonAdd={() => handleVisibilityAdd(true)}
				setSearch={(search: string) => {
					setSearch(search)
				}}
				placeholder="Search Users by Name, Email"
				changeNumberRows={(value) => {
					setPerPage(+value)
				}}
			/>

			<ClientsAdd open={openAdd} handleVisibility={handleVisibilityAdd} />
			<ClientsEdit open={openEdit} client={currClient} handleVisibility={handleVisibilityEdit} />
			<DialogRemove
				message={`Are you sure you want to remove ${currClient.name} from the clients list?`}
				open={openRemove}
				handleVisibility={handleVisibilityRemove}
				actionButtonYes={() => {
					removeClientMutation.mutate(currClient.id)
					handleVisibilityRemove(false)
				}}
				actionButtonCancel={() => handleVisibilityRemove(false)}
			/>
		</div>
	)
}
