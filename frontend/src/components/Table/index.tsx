import "./style.scss"

import { ChangeEvent, useEffect, useState } from "react"

import { Button } from "flowbite-react"

import { TableSizes } from "../../models/TableSizes"

import useDebounce from "../../hooks/useDebounce"

import { AiOutlinePlus as PlusIcon, AiOutlineSearch as SearchIcon } from "react-icons/ai"
import { HiFilter as FilterIcon } from "react-icons/hi"
import { SlArrowLeft as ArrowLeftIcon, SlArrowRight as ArrowRightIcon } from "react-icons/sl"

export interface TableProps {
	size?: TableSizes
	titles: any[]
	content: any[][]
	placeholder: string
	page: number
	maxPage: number
	previousPage: () => void
	nextPage: () => void
	buttonAdd?: () => void
	setSearch: (search: string) => void
	changeNumberRows: (value: string) => void
	filters?: React.ReactNode
}

export default function Table(props: TableProps) {
	let size = props.size
	if (!size) {
		size = TableSizes.LARGE
	}

	const [isFiltersVisible, setIsFiltersVisible] = useState(false)
	const [value, setValue] = useState<string>("")
	const debouncedValue = useDebounce<string>(value, 300)

	const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
		setValue(event.target.value)
	}

	useEffect(() => {
		props.setSearch(debouncedValue)
	}, [debouncedValue])

	const optionsNumberRows = [
		{
			value: "10",
			label: "10",
		},
		{
			value: "15",
			label: "15",
		},
		{
			value: "20",
			label: "20",
		},
	]

	return (
		<div className={`${size.size}`}>
			<div className="block bg-white border border-gray-200 rounded-lg shadow-md">
				<div className="myTableTop">
					<div className="flex gap-3">
						{props.filters && (
							<Button
								size="sm"
								color="dark"
								outline={true}
								className="!bg-[#8B83BA]"
								onClick={() => {
									setIsFiltersVisible((prev) => !prev)
								}}
							>
								<FilterIcon size={20} />
								Filter
							</Button>
						)}

						<div className="myTableSearch">
							<span>
								<SearchIcon size={25} />
							</span>
							<input
								type="text"
								name="search"
								placeholder={props.placeholder}
								onChange={handleChange}
							/>
						</div>
					</div>
					<div>
						{props.buttonAdd && (
							<Button
								onClick={() => {
									if (props.buttonAdd) props.buttonAdd()
								}}
								color="dark"
								className="!bg-white !hover:bg-white !text-black w-10 h-10"
							>
								<PlusIcon />
							</Button>
						)}
					</div>
				</div>

				<div>
					{props.filters && (
						<div
							className={
								isFiltersVisible
									? "w-full grid grid-cols-1 sm:grid-cols-2 gap-x-4 gap-y-2 p-3 my-4"
									: "hidden"
							}
						>
							{props.filters}
						</div>
					)}
				</div>

				<table className="myTable">
					<tbody>
						<tr>
							{props.titles.map((item, index) => (
								<th key={index}>{item}</th>
							))}
						</tr>
						{props.content.map((items, indexTr) => (
							<tr key={indexTr}>
								{items.map((item, indexTd) => (
									<td key={indexTd}>{item}</td>
								))}
							</tr>
						))}
					</tbody>
				</table>
				<div className="myTableBottom">
					<div className="flex">
						Row per page:
						<select
							className="cursor-pointer"
							onChange={(e) => {
								props.changeNumberRows(e.target.value)
							}}
						>
							{optionsNumberRows.map((o) => (
								<option key={o.value} value={o.value}>
									{o.label}
								</option>
							))}
						</select>
					</div>
					<div>
						{props.page}-{props.maxPage} of {props.maxPage}
					</div>
					<ArrowLeftIcon
						className="cursor-pointer"
						size={15}
						onClick={() => props.previousPage()}
					/>
					<ArrowRightIcon className="cursor-pointer" size={15} onClick={() => props.nextPage()} />
				</div>
			</div>
		</div>
	)
}
