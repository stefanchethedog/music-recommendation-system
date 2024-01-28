import { FC, useState, useEffect } from "react";
import { UserForm } from "../../components";
import axios from "axios";

import { UserList } from "../../components/User";

import { GET_ALL_USERS } from "../../endpoints";
import { UserProps } from "../../components/User/UserCard/User.component";

import "./User.styles.scss";
import useSnackbar from "../../Hooks/useSnackbar";

type UserPageProps = {
  userId: string | null,
}

const UserPage: FC<UserPageProps> = ({userId}) => {
  const [users, setUsers] = useState<
    Omit<UserProps, "className">[] | undefined
  >();

  const { createSnackbar } = useSnackbar({
    message: "Success",
    errorMessage: "Error, request failed",
  });

  const handleLoadUsers = async () => {
    await axios
      .get(GET_ALL_USERS)
      .then((res) => {
        setUsers(res.data);
        createSnackbar({ error: false });
      })
      .catch((err) => {
        createSnackbar({ error: true });
      });
  };

  return (
    <div className="user-page">
      <UserForm handleLoadUsers={handleLoadUsers} userId={userId}/>
      {users && (
        <UserList
          title="All users"
          usersData={users}
          className="user-page__user-list"
        />
      )}
    </div>
  );
};

export default UserPage;
