import  { useContext } from 'react'
import { JwtContext } from '../../routes/Root'

type Props = {}

const Vehicles = (props: Props) => {
  const {jwtLoginResponse,setJwtLoginResponse} = useContext(JwtContext)
  return (
    <div>Vehicles</div>
  )
}

export default Vehicles