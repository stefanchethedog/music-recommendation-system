import { FC, useState, useEffect } from "react";
import { SongList, UserForm } from "../../components";
import axios from "axios";

import { UserList } from "../../components/User";

import { GET_ALL_USERS, GET_RECOMMENDED_SONGS } from "../../endpoints";
import { UserProps } from "../../components/User/UserCard/User.component";

import "./User.styles.scss";
import useSnackbar from "../../Hooks/useSnackbar";
import { SongProps } from "../../components/Song/SongCard/Song.component";

type UserPageProps = {
  userId: string | null;
};

const UserPage: FC<UserPageProps> = ({ userId }) => {
  const [users, setUsers] = useState<
    Omit<UserProps, "className">[] | undefined
  >();
  const [recommendedSongs, setRecommendedSongs] = useState<
    Omit<SongProps, "className">[]
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

  const handleLoadRecommendedSongs = async () => {
    await axios
      .get(GET_RECOMMENDED_SONGS(String(userId)))
      .then((res) => {
        setRecommendedSongs(res.data);
        createSnackbar({ error: false });
      })
      .catch(() => {
        createSnackbar({ error: true });
      });
  };

  return (
    <div className="user-page">
      <UserForm
        handleLoadUsers={handleLoadUsers}
        handleLoadRecommendedSongs={handleLoadRecommendedSongs}
        userId={userId}
      />
      {users && users.length > 0 && (
        <UserList
          title="All users"
          usersData={users}
          className="user-page__user-list"
        />
      )}
      {recommendedSongs && recommendedSongs.length > 0 && (
        <SongList
          title="Recommended songs"
          songData={recommendedSongs}
          className="user-page__user-list"
        />
      )}
    </div>
  );
};

export default UserPage;
