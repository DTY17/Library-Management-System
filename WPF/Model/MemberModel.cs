using System.Collections.Generic;
using WPF.Dto;
using WPF.Repository;

namespace WPF.Model
{
    internal class MemberModel
    {
        private readonly MemberRepository repo;

        public MemberModel()
        {
            repo = new MemberRepository();
        }

        public int getMemberCount() => repo.getMemberCountRepo();

        public List<MemberDto> getMemberList() => repo.getMemberListRepo();

        public bool saveData(MemberDto member) => repo.saveDataRepo(member);

        public List<MemberDto> getAllMemberList() => repo.getAllMemberListRepo();

        public bool updateData(MemberDto member) => repo.updateDataRepo(member);

        public bool deleteData(MemberDto member) => repo.deleteDataRepo(member);
    }
}
