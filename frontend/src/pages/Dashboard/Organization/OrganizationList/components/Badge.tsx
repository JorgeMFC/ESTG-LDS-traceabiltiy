import { FaTrash } from "react-icons/fa"

interface BadgeContentProps {
	id: number | string
	name: string
}

interface BadgeProps {
	badgeContent: BadgeContentProps
	onRemove: (id: number | string) => void
}

export const Badge = (props: BadgeProps) => {
	return (
		<div className="flex badge cursor-pointer">
			<div className="bg-slate-200 rounded transition-opacity">
				<span className="block py-2.5 px-5 text-center text-sm font-medium text-gray-700">
					{props.badgeContent.name}
				</span>
			</div>
			<button
				className="text-white bg-gray-500 border border-gray-500 hover:bg-gray-700 focus:outline-none font-medium rounded-tr rounded-br text-sm px-1.5 text-center transition badgeActions hidden"
				onClick={() => {
					props.onRemove(props.badgeContent.id)
				}}
			>
				<FaTrash />
			</button>
		</div>
	)
}
