import { useState, useEffect, useRef } from "react"

/* This hook is used when a component need to toggle it's visibility when clicked outside, for example a select box */
/* Example
	const { ref, isComponentVisible, setIsComponentVisible } = useComponentVisible(false)

	On the parent div
	<div className="flex items-center gap-2" ref={ref}>

	Then use the variables isComponentVisible and setIsComponentVisible
*/
export default function useComponentVisible(initialIsVisible: boolean) {
	const [isComponentVisible, setIsComponentVisible] = useState(initialIsVisible)
	const ref = useRef<any>(null)

	const handleClickOutside = (event: any) => {
		if (ref.current && !ref.current.contains(event.target)) {
			setIsComponentVisible(false)
		}
	}

	useEffect(() => {
		document.addEventListener("click", handleClickOutside, true)
		return () => {
			document.removeEventListener("click", handleClickOutside, true)
		}
	}, [])

	return { ref, isComponentVisible, setIsComponentVisible }
}
