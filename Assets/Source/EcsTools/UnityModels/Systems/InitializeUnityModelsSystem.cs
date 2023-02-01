using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTools.UnityModels
{
	public readonly struct InitializeUnityModelsSystem : IEcsPreInitSystem
	{
		private readonly EcsFilterInject<Inc<TransformModel>> _transformFilter;
		private readonly EcsPoolInject<TransformModel> _transformMdlPool;

		public void PreInit(IEcsSystems systems)
		{
			foreach (var entity in _transformFilter.Value)
			{
				ref var transformMdl = ref _transformMdlPool.Value.Get(entity);

				transformMdl.initialLocalPos = transformMdl.transform.localPosition;
				transformMdl.initialLocalRot = transformMdl.transform.localRotation;
			}
		}
	}
}